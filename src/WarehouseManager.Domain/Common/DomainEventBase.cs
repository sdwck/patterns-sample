namespace WarehouseManager.Domain.Common;

public abstract record DomainEventBase : IDomainEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}