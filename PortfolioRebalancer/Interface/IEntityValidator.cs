using PortfolioRebalancer.Models;


namespace PortfolioRebalancer.Interface
{
    public interface IEntityValidator
    {
        RebalanceResult Validate(List<Entity> entities);
    }
}
