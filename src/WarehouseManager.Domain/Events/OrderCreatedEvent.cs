using WarehouseManager.Domain.Common;

namespace WarehouseManager.Domain.Events;

public record OrderCreatedEvent(Guid OrderId, Guid CustomerId, decimal TotalAmount) : DomainEventBase;