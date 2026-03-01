namespace WarehouseManager.Application.Common.Interfaces;

public interface IEmailNotificationService
{
    Task SendAsync(string recipient, string subject, string body, CancellationToken ct = default);
}