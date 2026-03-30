using WarehouseManager.Domain.Common;

namespace WarehouseManager.Domain.Entities;


public abstract class WarehouseComponent : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public virtual void Add(WarehouseComponent component)
    {
        throw new NotSupportedException();
    }

    public virtual void Remove(WarehouseComponent component)
    {
        throw new NotSupportedException();
    }

    public virtual WarehouseComponent GetChild(int index)
    {
        throw new NotSupportedException();
    }

    public abstract int GetTotalProductCount();
}

public class CompositeProduct : WarehouseComponent
{
    public override int GetTotalProductCount()
    {
        return 1;
    }
}

public class Category : WarehouseComponent
{
    public string? Description { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }

    private readonly List<WarehouseComponent> _children = new();
    
    public IReadOnlyCollection<WarehouseComponent> Children => _children.AsReadOnly();

    public override void Add(WarehouseComponent component)
    {
        _children.Add(component);
    }

    public override void Remove(WarehouseComponent component)
    {
        _children.Remove(component);
    }

    public override WarehouseComponent GetChild(int index)
    {
        return _children[index];
    }

    public override int GetTotalProductCount()
    {
        int count = 0;
        foreach (var child in _children)
        {
            count += child.GetTotalProductCount();
        }
        return count;
    }

    public IEnumerable<WarehouseComponent> GetAllDescendants()
    {
        foreach (var child in _children)
        {
            yield return child;
            if (child is Category category)
            {
                foreach (var descendant in category.GetAllDescendants())
                {
                    yield return descendant;
                }
            }
        }
    }

    public int GetDepth()
    {
        return ParentCategory is null ? 0 : 1 + ParentCategory.GetDepth();
    }
}