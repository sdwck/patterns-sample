using Microsoft.Extensions.Logging;
using WarehouseManager.Application.Common.Interfaces;

namespace WarehouseManager.Infrastructure.Notifications;

public class SmtpEmailService : IEmailNotificationService
{
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(ILogger<SmtpEmailService> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(string recipient, string subject, string body, CancellationToken ct = default)
    {
        _logger.LogInformation("[SMTP] To: {To}, Subject: {Subj}, Body: {Body}", recipient, subject, body);
        return Task.CompletedTask;
    }
}