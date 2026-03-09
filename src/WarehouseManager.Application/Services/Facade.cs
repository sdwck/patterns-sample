using WarehouseManager.Application.Common.Interfaces;
using WarehouseManager.Application.DTOs;
using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Entities;
using WarehouseManager.Domain.Interfaces;
using WarehouseManager.Domain.Services;
using WarehouseManager.Domain.Strategies;

namespace WarehouseManager.Application.Services;

public record OrderItemRequest(Guid ProductId, int Quantity);

public class Facade
{
    private readonly INotificationFactory _notificationFactory; 
    private readonly IPaymentService _paymentService;
    private readonly IUnitOfWork _uow;

    public Facade(
        IUnitOfWork uow,
        IPaymentService paymentService,
        INotificationFactory notificationFactory)
    {
        _uow = uow;
        _paymentService = paymentService;
        _notificationFactory = notificationFactory;
    }

    public async Task<Result<Guid>> PlaceOrderAsync(
        Guid customerId,
        string? shippingAddress,
        List<OrderItemRequest> items,
        string discountStrategyName = "None",
        CancellationToken ct = default)
    {
        var customer = await _uow.Customers.GetByIdAsync(customerId, ct);
        if (customer is null)
            return Result.Failure<Guid>("Customer not found.");

        var lineItems = new List<(Product, Stock, int)>();
        foreach (var item in items)
        {
            var product = await _uow.Products.GetByIdAsync(item.ProductId, ct);
            if (product is null)
                return Result.Failure<Guid>($"Product {item.ProductId} not found.");

            var stock = await _uow.Stock.GetByProductIdAsync(item.ProductId, ct);
            if (stock is null)
                return Result.Failure<Guid>($"Stock record for '{product.Name}' not found.");

            lineItems.Add((product, stock, item.Quantity));
        }

        var strategy = DiscountStrategyRegistry.Instance.GetStrategy(discountStrategyName);

        var orderResult = OrderDomainService.CreateOrder(customer, shippingAddress, lineItems, strategy);
        if (orderResult.IsFailure)
            return Result.Failure<Guid>(orderResult.Error!);

        var order = orderResult.Value!;

        var paymentResult = await _paymentService.ChargeAsync(order.Id, order.TotalAmount, ct);
        if (!paymentResult.Success)
            return Result.Failure<Guid>($"Payment failed: {paymentResult.Error}");

        await _uow.Orders.AddAsync(order, ct);
        await _uow.SaveChangesAsync(ct);

        var emailService = _notificationFactory.CreateEmailService();
        await emailService.SendAsync(
            customer.Email,
            "Order Confirmation",
            $"Dear {customer.FullName}, your order {order.OrderNumber} " +
            $"for ${order.TotalAmount:F2} has been placed. " +
            $"Transaction: {paymentResult.TransactionId}",
            ct);

        return Result.Success(order.Id);
    }

    public async Task<Result<OrderDto>> GetOrderDetailsAsync(Guid orderId, CancellationToken ct = default)
    {
        var order = await _uow.Orders.GetWithItemsAsync(orderId, ct);
        if (order is null)
            return Result.Failure<OrderDto>("Order not found.");

        return Result.Success(new OrderDto(
            order.Id, order.OrderNumber, order.CustomerId,
            order.Customer?.FullName ?? "", order.Status.ToString(),
            order.TotalAmount, order.ShippingAddress, order.CreatedAt,
            order.Items.Select(i => new OrderItemDto(
                i.Id, i.ProductId, i.Product?.Name ?? "",
                i.Quantity, i.UnitPrice, i.Total)).ToList()));
    }
}