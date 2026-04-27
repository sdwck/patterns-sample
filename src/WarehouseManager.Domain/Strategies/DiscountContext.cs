namespace WarehouseManager.Domain.Strategies;

public class DiscountContext
{
    private readonly IDiscountStrategy _strategy;

    public DiscountContext(IDiscountStrategy strategy)
    {
        _strategy = strategy;
    }

    public decimal CalculateDiscount(decimal originalPrice, int quantity)
    {
        return _strategy.CalculateDiscount(originalPrice, quantity);
    }
}