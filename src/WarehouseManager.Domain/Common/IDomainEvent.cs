namespace WarehouseManager.Domain.Common;

public interface IDomainEvent
{
    DateTime OccurredAt { get; }
}