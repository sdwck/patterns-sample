namespace WarehouseManager.Domain.Strategies;

public interface IDiscountStrategy
{
    string Name { get; }
    decimal CalculateDiscount(decimal originalPrice, int quantity);
}