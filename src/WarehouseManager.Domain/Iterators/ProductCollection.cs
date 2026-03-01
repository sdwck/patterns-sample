using WarehouseManager.Domain.Entities;

namespace WarehouseManager.Domain.Iterators;

public class ProductCollection : IIterableCollection<Product>
{
    private readonly List<Product> _products;

    public ProductCollection(
        IEnumerable<Product> products,
        Func<Product, bool>? filter = null,
        Func<Product, object>? orderBy = null,
        bool descending = false)
    {
        var source = products;

        if (filter is not null)
            source = source.Where(filter);

        if (orderBy is not null)
            source = descending ? source.OrderByDescending(orderBy) : source.OrderBy(orderBy);

        _products = source.ToList();
    }

    public int Count => _products.Count;

    public IIterator<Product> CreateIterator()
    {
        return new ProductIterator(_products);
    }

    public IIterator<Product> CreatePagedIterator(int page, int pageSize)
    {
        var paged = _products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return new ProductIterator(paged);
    }

    private class ProductIterator : IIterator<Product>
    {
        private readonly List<Product> _items;
        private int _position = -1;

        public ProductIterator(List<Product> items)
        {
            _items = items;
        }

        public bool HasNext()
        {
            return _position + 1 < _items.Count;
        }

        public Product Current =>
            _position >= 0 && _position < _items.Count
                ? _items[_position]
                : throw new InvalidOperationException("Iterator not positioned on valid element.");

        public void MoveNext()
        {
            if (!HasNext()) throw new InvalidOperationException("No more elements.");
            _position++;
        }

        public void Reset()
        {
            _position = -1;
        }
    }
}