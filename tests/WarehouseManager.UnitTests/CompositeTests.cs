using FluentAssertions;
using WarehouseManager.Domain.Entities;

namespace WarehouseManager.UnitTests;

public class CompositeTests
{
    [Fact]
    public void GetAllDescendants_ReturnsNestedCategories()
    {
        var root = new Category { Name = "Root" };
        var c1 = new Category { Name = "C1", ParentCategory = root };
        var c2 = new Category { Name = "C2", ParentCategory = root };
        var gc = new Category { Name = "GC", ParentCategory = c2 };
        root.SubCategories.Add(c1);
        root.SubCategories.Add(c2);
        c2.SubCategories.Add(gc);

        root.GetAllDescendants().Should().HaveCount(3);
    }

    [Fact]
    public void GetDepth_ReturnsCorrectDepth()
    {
        var root = new Category { Name = "R" };
        var child = new Category { Name = "C", ParentCategory = root };
        var gc = new Category { Name = "GC", ParentCategory = child };

        root.GetDepth().Should().Be(0);
        child.GetDepth().Should().Be(1);
        gc.GetDepth().Should().Be(2);
    }
}