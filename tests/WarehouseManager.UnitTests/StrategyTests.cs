using FluentAssertions;
using WarehouseManager.Domain.Strategies;

namespace WarehouseManager.UnitTests;

public class StrategyTests
{
    [Fact]
    public void NoDiscount_ReturnsZero()
    {
        new NoDiscountStrategy().CalculateDiscount(100, 5).Should().Be(0);
    }

    [Fact]
    public void Percentage_CalculatesCorrectly()
    {
        new PercentageDiscountStrategy(10).CalculateDiscount(100, 2).Should().Be(20);
    }

    [Fact]
    public void Bulk_BelowMinimum_ReturnsZero()
    {
        new BulkDiscountStrategy(10, 15).CalculateDiscount(100, 5).Should().Be(0);
    }

    [Fact]
    public void Bulk_AboveMinimum_CalculatesDiscount()
    {
        new BulkDiscountStrategy(10, 15).CalculateDiscount(100, 10).Should().Be(150);
    }
}