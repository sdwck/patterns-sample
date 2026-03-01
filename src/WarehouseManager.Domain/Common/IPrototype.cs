namespace WarehouseManager.Domain.Common;

public interface IPrototype<out T>
{
    T Clone();
}