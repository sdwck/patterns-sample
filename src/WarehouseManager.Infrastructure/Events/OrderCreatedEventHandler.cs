using Microsoft.Extensions.Logging;
using WarehouseManager.Application.Common.Interfaces;
using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Events;

namespace WarehouseManager.Infrastructure.Events;

public class OrderCreatedEventHandler : IDomainEventHandler<OrderCreatedEvent>
{
    private readonly ILogger<OrderCreatedEventHandler> _logger;
    private readonly INotificationFactory _notificationFactory;

    public OrderCreatedEventHandler(
        ILogger<OrderCreatedEventHandler> logger,
        INotificationFactory notificationFactory)
    {
        _logger = logger;
        _notificationFactory = notificationFactory;
    }

    public async Task HandleAsync(OrderCreatedEvent domainEvent, CancellationToken ct = default)
    {
        _logger.LogInformation(
            "Order {OrderId} created for customer {CustomerId}, total: {Total}",
            domainEvent.OrderId, domainEvent.CustomerId, domainEvent.TotalAmount);

        var emailService = _notificationFactory.CreateEmailService();
        await emailService.SendAsync(
            domainEvent.CustomerId.ToString(),
            "Order Confirmation",
            $"Your order {domainEvent.OrderId} has been placed. Total: ${domainEvent.TotalAmount}",
            ct);
    }
}