namespace WarehouseManager.Domain.Iterators;

public interface IIterableCollection<T>
{
    IIterator<T> CreateIterator();
}