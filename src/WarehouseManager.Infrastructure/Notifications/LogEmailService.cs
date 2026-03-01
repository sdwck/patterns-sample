using Microsoft.Extensions.Logging;
using WarehouseManager.Application.Common.Interfaces;

namespace WarehouseManager.Infrastructure.Notifications;

public class LogEmailService : IEmailNotificationService
{
    private readonly ILogger<LogEmailService> _logger;

    public LogEmailService(ILogger<LogEmailService> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(string recipient, string subject, string body, CancellationToken ct = default)
    {
        _logger.LogInformation("[LOG-EMAIL] To: {To}, Subject: {Subj}, Body: {Body}", recipient, subject, body);
        return Task.CompletedTask;
    }
}