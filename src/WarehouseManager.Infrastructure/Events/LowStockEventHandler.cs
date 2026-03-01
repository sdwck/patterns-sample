using Microsoft.Extensions.Logging;
using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Events;

namespace WarehouseManager.Infrastructure.Events;

public class LowStockEventHandler : IDomainEventHandler<LowStockEvent>
{
    private readonly ILogger<LowStockEventHandler> _logger;

    public LowStockEventHandler(ILogger<LowStockEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(LowStockEvent e, CancellationToken ct = default)
    {
        _logger.LogWarning("LOW STOCK: Product {ProductId}: {Qty}/{Reorder}", e.ProductId, e.CurrentQuantity,
            e.ReorderLevel);
        return Task.CompletedTask;
    }
}