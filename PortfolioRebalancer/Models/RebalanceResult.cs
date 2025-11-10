
namespace PortfolioRebalancer.Models
{
    public class RebalanceResult
    {
        public bool Success { get; }
        public string Message { get; }
        public List<Entity> Entities { get; }

        public RebalanceResult(bool success, string message, List<Entity> entities)
        {
            Success = success;
            Message = message;
            Entities = entities;
        }
    }
}
