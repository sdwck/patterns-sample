namespace WarehouseManager.Domain.Iterators;

public interface IIterator<out T>
{
    T Current { get; }
    bool HasNext();
    void MoveNext();
    void Reset();
}