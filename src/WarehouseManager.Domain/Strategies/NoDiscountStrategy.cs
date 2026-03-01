namespace WarehouseManager.Domain.Strategies;

public class NoDiscountStrategy : IDiscountStrategy
{
    public string Name => "None";

    public decimal CalculateDiscount(decimal originalPrice, int quantity)
    {
        return 0;
    }
}