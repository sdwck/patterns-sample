using FluentAssertions;
using WarehouseManager.Domain.Entities;
using WarehouseManager.Domain.Events;

namespace WarehouseManager.UnitTests;

public class OrderStateTests
{
    private static Order MakeOrder()
    {
        return new Order { OrderNumber = "ORD-TEST", CustomerId = Guid.NewGuid() };
    }

    [Fact]
    public void NewOrder_IsPending()
    {
        MakeOrder().Status.ToString().Should().Be("Pending");
    }

    [Fact]
    public void FullLifecycle_Works()
    {
        var o = MakeOrder();
        o.Confirm().IsSuccess.Should().BeTrue();
        o.StartProcessing().IsSuccess.Should().BeTrue();
        o.Ship().IsSuccess.Should().BeTrue();
        o.Deliver().IsSuccess.Should().BeTrue();
        o.Status.ToString().Should().Be("Delivered");
    }

    [Fact]
    public void Pending_Ship_Fails()
    {
        MakeOrder().Ship().IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Delivered_Cancel_Fails()
    {
        var o = MakeOrder();
        o.Confirm();
        o.StartProcessing();
        o.Ship();
        o.Deliver();
        o.Cancel().IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Pending_Cancel_Succeeds()
    {
        MakeOrder().Cancel().IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Confirm_RaisesStatusChangedEvent()
    {
        var o = MakeOrder();
        o.Confirm();
        o.DomainEvents.Should().ContainSingle().Which.Should().BeOfType<OrderStatusChangedEvent>();
    }
}