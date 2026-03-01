using Microsoft.Extensions.Logging;
using WarehouseManager.Application.Common.Interfaces;

namespace WarehouseManager.Infrastructure.Notifications;

public class LogNotificationFactory : INotificationFactory
{
    private readonly ILoggerFactory _loggerFactory;

    public LogNotificationFactory(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public IEmailNotificationService CreateEmailService()
    {
        return new LogEmailService(_loggerFactory.CreateLogger<LogEmailService>());
    }

    public ISmsNotificationService CreateSmsService()
    {
        return new LogSmsService(_loggerFactory.CreateLogger<LogSmsService>());
    }
}