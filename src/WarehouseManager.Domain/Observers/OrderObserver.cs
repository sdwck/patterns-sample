namespace WarehouseManager.Domain.Observers;

public class OrderObserver : Observer
{
    private readonly OrderSubject _subject;
    private string _observerState = string.Empty;

    public OrderObserver(OrderSubject subject)
    {
        _subject = subject;
    }

    public override void Update()
    {
        _observerState = _subject.GetState();
    }

    public string GetObserverState()
    {
        return _observerState;
    }
}