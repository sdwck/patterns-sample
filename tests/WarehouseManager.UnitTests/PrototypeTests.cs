using FluentAssertions;
using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Entities;

namespace WarehouseManager.UnitTests;

public class PrototypeTests
{
    [Fact]
    public void Clone_ShouldCreateExactCopyWithNewId()
    {
        var original = CreateOrderWithItems();

        var clone = original.Clone();

        clone.Id.Should().NotBe(original.Id);
        clone.CustomerId.Should().Be(original.CustomerId);
        clone.ShippingAddress.Should().Be(original.ShippingAddress);
        clone.TotalAmount.Should().Be(original.TotalAmount);
    }

    [Fact]
    public void Clone_ShouldDeepCopyItems()
    {
        var original = CreateOrderWithItems();

        var clone = original.Clone();

        clone.Items.Should().HaveCount(original.Items.Count);
        foreach (var clonedItem in clone.Items) original.Items.Should().NotContain(i => i.Id == clonedItem.Id);
    }

    [Fact]
    public void Clone_ShouldPreserveItemData()
    {
        var original = CreateOrderWithItems();

        var clone = original.Clone();

        var origItems = original.Items.ToList();
        var cloneItems = clone.Items.ToList();

        for (var i = 0; i < origItems.Count; i++)
        {
            cloneItems[i].ProductId.Should().Be(origItems[i].ProductId);
            cloneItems[i].Quantity.Should().Be(origItems[i].Quantity);
            cloneItems[i].UnitPrice.Should().Be(origItems[i].UnitPrice);
        }
    }

    [Fact]
    public void Clone_ModifyingCloneShouldNotAffectOriginal()
    {
        var original = CreateOrderWithItems();
        var originalItemCount = original.Items.Count;

        var clone = original.Clone();
        clone.Items.Add(new OrderItem { ProductId = Guid.NewGuid(), Quantity = 99, UnitPrice = 999 });

        original.Items.Should().HaveCount(originalItemCount);
    }

    [Fact]
    public void CloneAsNewOrder_ShouldHaveNewOrderNumber()
    {
        var original = CreateOrderWithItems();

        var clone = original.CloneAsNewOrder();

        clone.OrderNumber.Should().NotBe(original.OrderNumber);
        clone.OrderNumber.Should().StartWith("ORD-");
    }

    [Fact]
    public void CloneAsNewOrder_ShouldResetStatusToPending()
    {
        var original = CreateOrderWithItems();
        original.Confirm();

        var clone = original.CloneAsNewOrder();

        clone.Status.ToString().Should().Be("Pending");
    }

    [Fact]
    public void OrderItem_Clone_ShouldCopyData()
    {
        var item = new OrderItem
        {
            ProductId = Guid.NewGuid(),
            Quantity = 5,
            UnitPrice = 42.50m
        };

        var clone = item.Clone();

        clone.Id.Should().NotBe(item.Id);
        clone.ProductId.Should().Be(item.ProductId);
        clone.Quantity.Should().Be(item.Quantity);
        clone.UnitPrice.Should().Be(item.UnitPrice);
    }

    [Fact]
    public void Order_ImplementsIPrototype()
    {
        var order = CreateOrderWithItems();
        order.Should().BeAssignableTo<IPrototype<Order>>();
    }

    [Fact]
    public void OrderItem_ImplementsIPrototype()
    {
        var item = new OrderItem();
        item.Should().BeAssignableTo<IPrototype<OrderItem>>();
    }

    private static Order CreateOrderWithItems()
    {
        var order = new Order
        {
            OrderNumber = "ORD-ORIGINAL",
            CustomerId = Guid.NewGuid(),
            ShippingAddress = "123 Test Street"
        };

        order.Items.Add(new OrderItem { ProductId = Guid.NewGuid(), Quantity = 2, UnitPrice = 49.99m });
        order.Items.Add(new OrderItem { ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 199.99m });
        order.CalculateTotal();

        return order;
    }
}