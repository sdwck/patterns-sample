namespace WarehouseManager.Domain.Observers;

public class OrderSubject : Subject
{
    private string _subjectState = string.Empty;

    public string GetState()
    {
        return _subjectState;
    }

    public void SetState(string state)
    {
        _subjectState = state;
        Notify();
    }
}