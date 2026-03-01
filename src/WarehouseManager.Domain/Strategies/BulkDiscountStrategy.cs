namespace WarehouseManager.Domain.Strategies;

public class BulkDiscountStrategy : IDiscountStrategy
{
    private readonly decimal _discountPercentage;
    private readonly int _minimumQuantity;

    public BulkDiscountStrategy(int minimumQuantity, decimal discountPercentage)
    {
        _minimumQuantity = minimumQuantity;
        _discountPercentage = discountPercentage;
    }

    public string Name => $"Bulk (min {_minimumQuantity}, {_discountPercentage}%)";

    public decimal CalculateDiscount(decimal originalPrice, int quantity)
    {
        if (quantity < _minimumQuantity) return 0;
        return originalPrice * quantity * (_discountPercentage / 100m);
    }
}