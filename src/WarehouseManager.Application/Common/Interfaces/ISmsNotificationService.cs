namespace WarehouseManager.Application.Common.Interfaces;

public interface ISmsNotificationService
{
    Task SendAsync(string recipient, string message, CancellationToken ct = default);
}