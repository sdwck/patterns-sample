using WarehouseManager.Domain.Common;

namespace WarehouseManager.Domain.Events;

public record OrderStatusChangedEvent(Guid OrderId, string OldStatus, string NewStatus) : DomainEventBase;