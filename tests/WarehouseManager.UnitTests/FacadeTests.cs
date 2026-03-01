using FluentAssertions;
using Moq;
using WarehouseManager.Application.Common.Interfaces;
using WarehouseManager.Application.Services;
using WarehouseManager.Domain.Entities;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.UnitTests;

public class FacadeTests
{
    private readonly Mock<IEmailNotificationService> _emailServiceMock;
    private readonly Facade _facade;
    private readonly Mock<INotificationFactory> _notifFactoryMock;
    private readonly Mock<IPaymentService> _paymentMock;
    private readonly Mock<IUnitOfWork> _uowMock;

    public FacadeTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _paymentMock = new Mock<IPaymentService>();
        _notifFactoryMock = new Mock<INotificationFactory>();
        _emailServiceMock = new Mock<IEmailNotificationService>();

        _notifFactoryMock.Setup(f => f.CreateEmailService()).Returns(_emailServiceMock.Object);

        _facade = new Facade(
            _uowMock.Object,
            _paymentMock.Object,
            _notifFactoryMock.Object);
    }

    [Fact]
    public async Task PlaceOrderAsync_WhenCustomerNotFound_ShouldFail()
    {
        _uowMock.Setup(u => u.Customers.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer?)null);

        var result = await _facade.PlaceOrderAsync(
            Guid.NewGuid(), null,
            new List<OrderItemRequest> { new(Guid.NewGuid(), 1) });

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("Customer not found");
    }

    [Fact]
    public async Task PlaceOrderAsync_WhenPaymentFails_ShouldFail()
    {
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        _uowMock.Setup(u => u.Customers.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Customer
                { Id = customerId, Email = "test@test.com", FirstName = "John", LastName = "Doe" });

        _uowMock.Setup(u => u.Products.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Product { Id = productId, Name = "Test", Price = 10m });

        var stock = new Stock { ProductId = productId, QuantityOnHand = 100, ReorderLevel = 5 };
        _uowMock.Setup(u => u.Stock.GetByProductIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(stock);

        _paymentMock.Setup(p => p.ChargeAsync(It.IsAny<Guid>(), It.IsAny<decimal>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PaymentResult(false, "", "Card declined"));

        var result = await _facade.PlaceOrderAsync(
            customerId, "123 Street",
            new List<OrderItemRequest> { new(productId, 2) });

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("Payment failed");
    }

    [Fact]
    public async Task PlaceOrderAsync_WhenSuccessful_ShouldSendEmail()
    {
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        _uowMock.Setup(u => u.Customers.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Customer
                { Id = customerId, Email = "john@test.com", FirstName = "John", LastName = "Doe" });

        _uowMock.Setup(u => u.Products.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Product { Id = productId, Name = "Widget", Price = 25m });

        var stock = new Stock { ProductId = productId, QuantityOnHand = 50, ReorderLevel = 5 };
        _uowMock.Setup(u => u.Stock.GetByProductIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(stock);

        _uowMock.Setup(u => u.Orders.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Order o, CancellationToken _) => o);

        _paymentMock.Setup(p => p.ChargeAsync(It.IsAny<Guid>(), It.IsAny<decimal>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PaymentResult(true, "TXN-123", null));

        var result = await _facade.PlaceOrderAsync(
            customerId, "456 Avenue",
            new List<OrderItemRequest> { new(productId, 3) });

        result.IsSuccess.Should().BeTrue();

        _emailServiceMock.Verify(
            e => e.SendAsync("john@test.com", "Order Confirmation", It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task PlaceOrderAsync_WhenProductNotFound_ShouldFail()
    {
        var customerId = Guid.NewGuid();

        _uowMock.Setup(u => u.Customers.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Customer { Id = customerId, Email = "test@test.com", FirstName = "A", LastName = "B" });

        _uowMock.Setup(u => u.Products.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        var result = await _facade.PlaceOrderAsync(
            customerId, null,
            new List<OrderItemRequest> { new(Guid.NewGuid(), 1) });

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("not found");
    }

    [Fact]
    public async Task PlaceOrderAsync_WhenInsufficientStock_ShouldFail()
    {
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        _uowMock.Setup(u => u.Customers.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Customer { Id = customerId, Email = "t@t.com", FirstName = "A", LastName = "B" });

        _uowMock.Setup(u => u.Products.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Product { Id = productId, Name = "Widget", Price = 10m });

        var stock = new Stock { ProductId = productId, QuantityOnHand = 2, ReorderLevel = 5 };
        _uowMock.Setup(u => u.Stock.GetByProductIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(stock);

        var result = await _facade.PlaceOrderAsync(
            customerId, null,
            new List<OrderItemRequest> { new(productId, 100) });

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("Insufficient stock");
    }
}