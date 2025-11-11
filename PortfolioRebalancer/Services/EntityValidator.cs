using PortfolioRebalancer.Interface;
using PortfolioRebalancer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioRebalancer.Services
{
    public class EntityValidator : IEntityValidator
    {
        public RebalanceResult Validate(List<Entity> entities)
        {
            try
            {
                //Null check for entities 
                if (entities == null || entities.Count == 0)
                    return new RebalanceResult(false, "No entities provided.", entities);

                // Validation for negative values
                var invalidEntities = entities.Where(e => e.Exposure < 0 || e.Capacity < 0).ToList();

                if (invalidEntities.Any())
                {
                    var invalidList = string.Join(", ", invalidEntities.Select(e => e.EntityId));
                    return new RebalanceResult(false, $"Invalid data: Entities with negative exposure or capacity detected. [{invalidList}]", entities);
                }
                return new RebalanceResult(true, "Validation passed.", entities);
            }
            catch (Exception ex)
            {
                return new RebalanceResult(false, $"Validation failed: {ex.Message}", entities);
            }
        }
    }
}
