namespace WarehouseManager.Domain.Iterators;

public interface IIterator<out T>
{
    bool HasNext();
    T Next();
}