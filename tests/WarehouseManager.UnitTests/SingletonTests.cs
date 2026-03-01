using FluentAssertions;
using WarehouseManager.Domain.Strategies;

namespace WarehouseManager.UnitTests;

public class SingletonTests
{
    [Fact]
    public void Instance_ReturnsSameInstance()
    {
        var a = DiscountStrategyRegistry.Instance;
        var b = DiscountStrategyRegistry.Instance;
        a.Should().BeSameAs(b);
    }

    [Fact]
    public void Registry_ContainsDefaultStrategies()
    {
        DiscountStrategyRegistry.Instance.GetAvailableStrategies().Should().Contain("None");
    }
}