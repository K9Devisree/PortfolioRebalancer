using Microsoft.Extensions.Logging;
using PortfolioRebalancer.Models;
using PortfolioRebalancer.Services;

namespace PortfolioRebalancer
{
  public  class Program
    {
        public static async Task Main()
        {
            try
            {
                Console.WriteLine("=== Dynamic Exposure Rebalancing ===\n");

                // Synchronous rebalance
                var entities = new List<Entity>
            {
                new("A", 80, 50, 1),
                new("B", 30, 60, 2),
                new("C", 20, 40, 3),
                new("D", 10, 20, 4)
            };

                PrintEntities("Before Rebalance", entities);
                var balancer = new ExposureBalancer(entities);
                var result = balancer.Rebalance();
                Console.WriteLine($"\n[Sync] {result.Message}");
                PrintEntities("After Sync Rebalance", result.Entities);


                // Async rebalance 
                //var entitiesAsync = new List<Entity>
                //{
                //    new("A", 80, 50, 1),
                //    new("B", 20, 60, 2),
                //    new("C", 15, 40, 3),
                //    new("D", 5, 20, 4)
                //};

                //var asyncBalancer = new ExposureBalancer(entitiesAsync);
                //var asyncResult = await asyncBalancer.RebalanceAsync();
                //Console.WriteLine($"\n[Async] {asyncResult.Message}");
                //PrintEntities("After Async Rebalance", asyncResult.Entities);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error during rebalancing.", ex.ToString());
            }
        }

        private static void PrintEntities(string title, List<Entity> entities)
        {
            Console.WriteLine($"\n--- {title} ---");
            foreach (var e in entities)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine($"Total Exposure: {entities.Sum(e => e.Exposure)}");
        }
    }
}