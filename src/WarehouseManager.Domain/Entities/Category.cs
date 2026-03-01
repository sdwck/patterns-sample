using WarehouseManager.Domain.Common;

namespace WarehouseManager.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public Guid? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; } = new List<Category>();
    public ICollection<Product> Products { get; set; } = new List<Product>();

    public IEnumerable<Category> GetAllDescendants()
    {
        foreach (var child in SubCategories)
        {
            yield return child;
            foreach (var descendant in child.GetAllDescendants())
                yield return descendant;
        }
    }

    public int GetDepth()
    {
        return ParentCategory is null ? 0 : 1 + ParentCategory.GetDepth();
    }

    public int GetTotalProductCount()
    {
        var count = Products.Count;
        foreach (var child in SubCategories)
            count += child.GetTotalProductCount();
        return count;
    }
}