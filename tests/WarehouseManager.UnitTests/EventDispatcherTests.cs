using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Events;
using WarehouseManager.Infrastructure.Events;

namespace WarehouseManager.UnitTests;

public class EventDispatcherTests
{
    [Fact]
    public async Task Dispatch_CallsHandler()
    {
        var handler = new TestHandler();
        var sp = new ServiceCollection()
            .AddSingleton<IDomainEventHandler<OrderCreatedEvent>>(handler)
            .BuildServiceProvider();

        var dispatcher = new EventDispatcher(sp);
        await dispatcher.DispatchAsync(new OrderCreatedEvent(Guid.NewGuid(), Guid.NewGuid(), 100));

        handler.Handled.Should().BeTrue();
    }

    [Fact]
    public async Task Dispatch_NoHandler_DoesNotThrow()
    {
        var sp = new ServiceCollection().BuildServiceProvider();
        var dispatcher = new EventDispatcher(sp);
        var act = () => dispatcher.DispatchAsync(new LowStockEvent(Guid.NewGuid(), 5, 10));
        await act.Should().NotThrowAsync();
    }

    private class TestHandler : IDomainEventHandler<OrderCreatedEvent>
    {
        public bool Handled { get; private set; }

        public Task HandleAsync(OrderCreatedEvent e, CancellationToken ct = default)
        {
            Handled = true;
            return Task.CompletedTask;
        }
    }
}