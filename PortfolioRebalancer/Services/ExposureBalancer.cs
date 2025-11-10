using PortfolioRebalancer;
using PortfolioRebalancer.Models;

namespace PortfolioRebalancer.Services
{
    public class ExposureBalancer
    {
        private  List<Entity> Entities;
        public ExposureBalancer(List<Entity> entities)
        {
            Entities = entities;
        }

        // Synchronous version
        public RebalanceResult Rebalance()
        {
            try
            {
                //Null check for entities 
                if (Entities == null || Entities.Count == 0)
                    return new RebalanceResult(false, "No entities provided.", Entities);

                decimal totalExposure = GetTotalExposure(); 
                decimal totalCapacity = Entities.Sum(e => e.Capacity);

                if (totalCapacity < totalExposure)
                {
                    return new RebalanceResult(false,$"Not possible to rebalance: total exposure ({totalExposure}) exceeds total capacity ({totalCapacity}).",Entities);
                }
                bool entityChanged = false;

                // Identify entities with excess exposure
                var excessEntities = Entities.Where(e => e.Exposure > e.Capacity)
                                     .OrderBy(e => e.Priority)
                                     .ToList();
                //  Identify receivers can accept more exposure
                var receiverEntities = Entities.Where(e => e.Exposure < e.Capacity)
                                        .OrderByDescending(e => e.Priority) //check for low priority
                                        .ToList();

                foreach (var excessEntity in excessEntities)
                {
                    decimal excess = excessEntity.Exposure - excessEntity.Capacity;
                    if (excess <= 0)
                        continue;
                    // Fill receivers one by one
                    foreach (var receiverEntity in receiverEntities)
                    {
                        if (excess <= 0)
                            break;

                        decimal availableCapacity = receiverEntity.Capacity - receiverEntity.Exposure;
                        if (availableCapacity <= 0)
                            continue;

                        decimal transferAmount = Math.Min(excess, availableCapacity);
                        excessEntity.Exposure -= transferAmount;
                        receiverEntity.Exposure += transferAmount;
                        excess -= transferAmount;
                        entityChanged = true;
                        Console.WriteLine($"Moved {transferAmount} from {excessEntity.EntityId} to {receiverEntity.EntityId}");
                    }

                    if (excessEntity.Exposure > excessEntity.Capacity)
                    {
                        return new RebalanceResult(false,$"Unable to fully rebalance. Entity {excessEntity.EntityId} remains over capacity.",Entities);
                    }
                }
                decimal newTotalExposure = GetTotalExposure(); 
                if (!IsValid())
                    return new RebalanceResult(false, "One or more entities exceed capacity after rebalancing.", Entities);

                if (Math.Abs(totalExposure - newTotalExposure) > 0.0001m)
                    return new RebalanceResult(false, "Total exposure mismatch after rebalancing.", Entities);

                if (!entityChanged)
                    return new RebalanceResult(true, "No redistribution needed (already balanced).", Entities);

                return new RebalanceResult(true, "Rebalancing completed successfully.", Entities);
            }
            catch (Exception ex)
            {
                return new RebalanceResult(false, $"Error during rebalancing: {ex.Message}", Entities);                               
            }
        }

        // Asynchronous version 
        public async Task<RebalanceResult> RebalanceAsync()
        {
            await Task.Delay(100);
            return await Task.Run(() => Rebalance());
        }

        public decimal GetTotalExposure() => Entities.Sum(e => e.Exposure);

        public bool IsValid() =>  Entities.All(e => e.Exposure <= e.Capacity);
    }
}
