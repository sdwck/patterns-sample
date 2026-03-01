using FluentAssertions;
using WarehouseManager.Domain.Entities;
using WarehouseManager.Domain.Services;
using WarehouseManager.Domain.Strategies;

namespace WarehouseManager.UnitTests;

public class OrderDomainServiceTests
{
    [Fact]
    public void CreateOrder_WithValidData_ShouldSucceed()
    {
        var customer = new Customer { FirstName = "John", LastName = "Doe", Email = "john@test.com" };
        var product = new Product { Name = "Widget", Price = 25m };
        var stock = new Stock { ProductId = product.Id, QuantityOnHand = 50, ReorderLevel = 5 };
        var strategy = new NoDiscountStrategy();

        var result = OrderDomainService.CreateOrder(
            customer, "123 Street",
            new List<(Product, Stock, int)> { (product, stock, 3) },
            strategy);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Items.Should().HaveCount(1);
        result.Value.TotalAmount.Should().Be(75m);
        result.Value.CustomerId.Should().Be(customer.Id);
    }

    [Fact]
    public void CreateOrder_WithInsufficientStock_ShouldFail()
    {
        var customer = new Customer { FirstName = "A", LastName = "B", Email = "a@b.com" };
        var product = new Product { Name = "Widget", Price = 10m };
        var stock = new Stock { ProductId = product.Id, QuantityOnHand = 2, ReorderLevel = 5 };

        var result = OrderDomainService.CreateOrder(
            customer, null,
            new List<(Product, Stock, int)> { (product, stock, 100) },
            new NoDiscountStrategy());

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("Insufficient stock");
    }

    [Fact]
    public void CreateOrder_WithBulkDiscount_ShouldApplyDiscount()
    {
        var customer = new Customer { FirstName = "A", LastName = "B", Email = "a@b.com" };
        var product = new Product { Name = "Widget", Price = 100m };
        var stock = new Stock { ProductId = product.Id, QuantityOnHand = 500, ReorderLevel = 5 };
        var strategy = new BulkDiscountStrategy(10, 10);

        var result = OrderDomainService.CreateOrder(
            customer, null,
            new List<(Product, Stock, int)> { (product, stock, 20) },
            strategy);

        result.IsSuccess.Should().BeTrue();
        result.Value!.TotalAmount.Should().BeLessThan(2000m);
    }

    [Fact]
    public void CreateOrder_ShouldDeductStock()
    {
        var customer = new Customer { FirstName = "A", LastName = "B", Email = "a@b.com" };
        var product = new Product { Name = "Widget", Price = 10m };
        var stock = new Stock { ProductId = product.Id, QuantityOnHand = 50, ReorderLevel = 5 };

        OrderDomainService.CreateOrder(
            customer, null,
            new List<(Product, Stock, int)> { (product, stock, 10) },
            new NoDiscountStrategy());

        stock.QuantityOnHand.Should().Be(40);
    }

    [Fact]
    public void CreateOrder_ShouldRaiseOrderCreatedEvent()
    {
        var customer = new Customer { FirstName = "A", LastName = "B", Email = "a@b.com" };
        var product = new Product { Name = "Widget", Price = 10m };
        var stock = new Stock { ProductId = product.Id, QuantityOnHand = 50, ReorderLevel = 5 };

        var result = OrderDomainService.CreateOrder(
            customer, null,
            new List<(Product, Stock, int)> { (product, stock, 5) },
            new NoDiscountStrategy());

        result.Value!.DomainEvents.Should().ContainSingle();
    }
}