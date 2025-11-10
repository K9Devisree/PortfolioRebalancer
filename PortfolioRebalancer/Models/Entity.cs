namespace PortfolioRebalancer.Models;

public class Entity
{
    public string EntityId { get; }
    public decimal Exposure { get; set; } // settable for rebalancing
    public decimal Capacity { get; }
    public int Priority { get; }

    public Entity(string entityId, decimal exposure, decimal capacity, int priority)
    {
        EntityId = entityId;
        Exposure = exposure;
        Capacity = capacity;
        Priority = priority;
    }

    public override string ToString() =>
        $"{EntityId}: Exposure={Exposure}, Capacity={Capacity}, Priority={Priority}";
}
