using Microsoft.Extensions.Logging;
using WarehouseManager.Application.Common.Interfaces;

namespace WarehouseManager.Infrastructure.Notifications;

public class SmtpNotificationFactory : INotificationFactory
{
    private readonly ILoggerFactory _loggerFactory;

    public SmtpNotificationFactory(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public IEmailNotificationService CreateEmailService()
    {
        return new SmtpEmailService(_loggerFactory.CreateLogger<SmtpEmailService>());
    }

    public ISmsNotificationService CreateSmsService()
    {
        return new TwilioSmsService(_loggerFactory.CreateLogger<TwilioSmsService>());
    }
}