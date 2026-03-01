using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WarehouseManager.Domain.Common;

namespace WarehouseManager.Infrastructure.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        //optionsBuilder.UseNpgsql("Host=localhost;Database=warehouse_db;Username=postgres;Password=postgres");
        optionsBuilder.UseInMemoryDatabase("WarehouseManagerDb");
        return new AppDbContext(optionsBuilder.Options, new NullEventDispatcher());
    }

    private class NullEventDispatcher : IEventDispatcher
    {
        public Task DispatchAsync(IDomainEvent domainEvent, CancellationToken ct = default)
        {
            return Task.CompletedTask;
        }
    }
}