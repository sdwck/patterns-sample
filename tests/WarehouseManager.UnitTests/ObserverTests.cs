using FluentAssertions;
using WarehouseManager.Domain.Observers;

namespace WarehouseManager.UnitTests;

public class ObserverTests
{
    [Fact]
    public void Observer_ShouldReceiveUpdatesFromSubject()
    {
        var subject = new OrderSubject();
        var observer = new OrderObserver(subject);
        subject.Attach(observer);

        subject.SetState("Processing");

        observer.GetObserverState().Should().Be("Processing");
    }
}