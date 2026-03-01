using WarehouseManager.Domain.Common;

namespace WarehouseManager.Domain.Events;

public record LowStockEvent(Guid ProductId, int CurrentQuantity, int ReorderLevel) : DomainEventBase;