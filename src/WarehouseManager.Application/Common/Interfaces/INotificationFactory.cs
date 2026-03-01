namespace WarehouseManager.Application.Common.Interfaces;

public interface INotificationFactory
{
    IEmailNotificationService CreateEmailService();
    ISmsNotificationService CreateSmsService();
}