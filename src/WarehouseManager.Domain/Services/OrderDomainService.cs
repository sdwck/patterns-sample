using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Entities;
using WarehouseManager.Domain.Strategies;

namespace WarehouseManager.Domain.Services;

public static class OrderDomainService
{
    public static Result<Order> CreateOrder(
        Customer customer,
        string? shippingAddress,
        List<(Product Product, Stock Stock, int Quantity)> lineItems,
        IDiscountStrategy discountStrategy)
    {
        var order = new Order
        {
            OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}",
            CustomerId = customer.Id,
            ShippingAddress = shippingAddress
        };

        foreach (var (product, stock, quantity) in lineItems)
        {
            var deductResult = stock.Deduct(quantity);
            if (deductResult.IsFailure)
                return Result.Failure<Order>(deductResult.Error!);

            var discount = discountStrategy.CalculateDiscount(product.Price, quantity);
            var effectivePrice = quantity > 0
                ? product.Price - discount / quantity
                : product.Price;

            order.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = quantity,
                UnitPrice = effectivePrice
            });
        }

        order.CalculateTotal();
        order.MarkAsCreated();

        return Result.Success(order);
    }
}