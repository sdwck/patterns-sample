namespace WarehouseManager.Domain.Strategies;

public sealed class DiscountStrategyRegistry
{
    private static DiscountStrategyRegistry? _instance;
    private static readonly Lock Lock = new();

    private readonly Dictionary<string, IDiscountStrategy> _strategies = new();

    private DiscountStrategyRegistry()
    {
        Register(new NoDiscountStrategy());
        Register(new PercentageDiscountStrategy(10));
        Register(new BulkDiscountStrategy(10, 5));
    }

    public static DiscountStrategyRegistry Instance
    {
        get
        {
            if (_instance is null)
                lock (Lock)
                {
                    _instance ??= new DiscountStrategyRegistry();
                }

            return _instance;
        }
    }

    public void Register(IDiscountStrategy strategy)
    {
        _strategies[strategy.Name] = strategy;
    }

    public IDiscountStrategy GetStrategy(string name)
    {
        return _strategies.TryGetValue(name, out var strategy) ? strategy : _strategies.Values.First();
    }

    public IReadOnlyList<string> GetAvailableStrategies()
    {
        return _strategies.Keys.ToList().AsReadOnly();
    }
}