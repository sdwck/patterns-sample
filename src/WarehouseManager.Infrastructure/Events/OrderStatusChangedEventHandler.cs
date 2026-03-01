using Microsoft.Extensions.Logging;
using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Events;

namespace WarehouseManager.Infrastructure.Events;

public class OrderStatusChangedEventHandler : IDomainEventHandler<OrderStatusChangedEvent>
{
    private readonly ILogger<OrderStatusChangedEventHandler> _logger;

    public OrderStatusChangedEventHandler(ILogger<OrderStatusChangedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(OrderStatusChangedEvent e, CancellationToken ct = default)
    {
        _logger.LogInformation("Order {OrderId}: {Old} → {New}", e.OrderId, e.OldStatus, e.NewStatus);
        return Task.CompletedTask;
    }
}