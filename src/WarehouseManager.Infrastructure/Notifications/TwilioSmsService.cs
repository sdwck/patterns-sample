using Microsoft.Extensions.Logging;
using WarehouseManager.Application.Common.Interfaces;

namespace WarehouseManager.Infrastructure.Notifications;

public class TwilioSmsService : ISmsNotificationService
{
    private readonly ILogger<TwilioSmsService> _logger;

    public TwilioSmsService(ILogger<TwilioSmsService> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(string recipient, string message, CancellationToken ct = default)
    {
        _logger.LogInformation("[TWILIO] To: {To}, Message: {Msg}", recipient, message);
        return Task.CompletedTask;
    }
}