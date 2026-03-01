using Microsoft.EntityFrameworkCore;
using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Entities;

namespace WarehouseManager.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    private readonly IEventDispatcher _eventDispatcher;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        IEventDispatcher eventDispatcher) : base(options)
    {
        _eventDispatcher = eventDispatcher;
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Stock> Stocks => Set<Stock>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<AppUser> Users => Set<AppUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                modelBuilder.Entity(entityType.ClrType).Ignore(nameof(BaseEntity.DomainEvents));

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries<BaseEntity>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            entry.Entity.ClearDomainEvents();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
            try
            {
                await _eventDispatcher.DispatchAsync(domainEvent, cancellationToken);
            }
            catch (Exception ex)
            {
                // ignored
            }

        return result;
    }
}