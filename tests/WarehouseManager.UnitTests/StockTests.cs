using FluentAssertions;
using WarehouseManager.Domain.Entities;
using WarehouseManager.Domain.Events;

namespace WarehouseManager.UnitTests;

public class StockTests
{
    [Fact]
    public void Deduct_Sufficient_DecreasesQuantity()
    {
        var stock = new Stock { QuantityOnHand = 50, ReorderLevel = 10 };
        stock.Deduct(10).IsSuccess.Should().BeTrue();
        stock.QuantityOnHand.Should().Be(40);
    }

    [Fact]
    public void Deduct_Insufficient_ReturnsFailure()
    {
        var stock = new Stock { QuantityOnHand = 5, ReorderLevel = 10 };
        stock.Deduct(10).IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Deduct_BelowReorderLevel_RaisesEvent()
    {
        var stock = new Stock { QuantityOnHand = 15, ReorderLevel = 10 };
        stock.Deduct(10);
        stock.DomainEvents.Should().ContainSingle().Which.Should().BeOfType<LowStockEvent>();
    }

    [Fact]
    public void Restock_IncreasesQuantity()
    {
        var stock = new Stock { QuantityOnHand = 10 };
        stock.Restock(25);
        stock.QuantityOnHand.Should().Be(35);
    }
}