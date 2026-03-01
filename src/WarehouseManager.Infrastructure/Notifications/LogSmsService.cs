using Microsoft.Extensions.Logging;
using WarehouseManager.Application.Common.Interfaces;

namespace WarehouseManager.Infrastructure.Notifications;

public class LogSmsService : ISmsNotificationService
{
    private readonly ILogger<LogSmsService> _logger;

    public LogSmsService(ILogger<LogSmsService> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(string recipient, string message, CancellationToken ct = default)
    {
        _logger.LogInformation("[LOG-SMS] To: {To}, Message: {Msg}", recipient, message);
        return Task.CompletedTask;
    }
}