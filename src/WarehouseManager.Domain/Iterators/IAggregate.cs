namespace WarehouseManager.Domain.Iterators;

public interface IAggregate<out T>
{
    IIterator<T> CreateIterator();
}