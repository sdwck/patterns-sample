using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManager.Domain.Entities;

namespace WarehouseManager.Infrastructure.Persistence.Configurations;

public class WarehouseComponentConfiguration : IEntityTypeConfiguration<WarehouseComponent>
{
    public void Configure(EntityTypeBuilder<WarehouseComponent> builder)
    {
        builder.ToTable("WarehouseComponents");
        builder.HasKey(w => w.Id);
        builder.Property(w => w.Name).HasMaxLength(100).IsRequired();
        
        builder.HasDiscriminator<string>("ComponentType")
            .HasValue<Category>("Category")
            .HasValue<CompositeProduct>("CompositeProduct");
    }
}