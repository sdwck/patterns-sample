using FluentAssertions;
using WarehouseManager.Domain.Entities;
using WarehouseManager.Domain.Iterators;

namespace WarehouseManager.UnitTests;

public class IteratorTests
{
    private static List<Product> Products()
    {
        return
        [
            new() { Name = "Alpha", Price = 30, Sku = "A" },
            new() { Name = "Beta", Price = 10, Sku = "B" },
            new() { Name = "Charlie", Price = 20, Sku = "C" },
            new() { Name = "Delta", Price = 50, Sku = "D" },
            new() { Name = "Echo", Price = 40, Sku = "E" }
        ];
    }

    [Fact]
    public void IterateAll_ReturnsFive()
    {
        var it = new ProductCollection(Products()).CreateIterator();
        var n = 0;
        while (it.HasNext())
        {
            it.MoveNext();
            n++;
        }

        n.Should().Be(5);
    }

    [Fact]
    public void WithFilter_FiltersCorrectly()
    {
        new ProductCollection(Products(), p => p.Price > 25).Count.Should().Be(3);
    }

    [Fact]
    public void WithSorting_SortsByPrice()
    {
        var it = new ProductCollection(Products(), orderBy: p => p.Price).CreateIterator();
        var prices = new List<decimal>();
        while (it.HasNext())
        {
            it.MoveNext();
            prices.Add(it.Current.Price);
        }

        prices.Should().BeInAscendingOrder();
    }

    [Fact]
    public void PagedIterator_ReturnsCorrectPage()
    {
        var it = new ProductCollection(Products(), orderBy: p => p.Name)
            .CreatePagedIterator(2, 2);
        var names = new List<string>();
        while (it.HasNext())
        {
            it.MoveNext();
            names.Add(it.Current.Name);
        }

        names.Should().HaveCount(2);
        names[0].Should().Be("Charlie");
    }

    [Fact]
    public void Reset_Restarts()
    {
        var it = new ProductCollection(Products()).CreateIterator();
        it.MoveNext();
        it.MoveNext();
        it.Reset();
        it.MoveNext();
        it.Current.Name.Should().Be(Products()[0].Name);
    }
}