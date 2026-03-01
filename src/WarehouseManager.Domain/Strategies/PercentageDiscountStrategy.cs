namespace WarehouseManager.Domain.Strategies;

public class PercentageDiscountStrategy : IDiscountStrategy
{
    private readonly decimal _percentage;

    public PercentageDiscountStrategy(decimal percentage)
    {
        _percentage = percentage;
    }

    public string Name => $"Percentage ({_percentage}%)";

    public decimal CalculateDiscount(decimal originalPrice, int quantity)
    {
        return originalPrice * quantity * (_percentage / 100m);
    }
}