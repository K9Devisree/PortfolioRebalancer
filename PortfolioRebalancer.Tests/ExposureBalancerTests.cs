using PortfolioRebalancer.Models;
using PortfolioRebalancer.Services;


namespace PortfolioRebalancer.Tests
{
    public class ExposureBalancerTests
    {
        [Fact(DisplayName = "Rebalance should correctly redistribute exposures")]
        public void Rebalance_ShouldRedistributeExposuresCorrectly()
        {
            // Arrange
            var entities = new List<Entity>
            {
                new("A", 70, 50, 1),
                new("B", 30, 60, 2),
                new("C", 20, 40, 3),
                new("D", 10, 20, 4)
            };

            var balancer = new ExposureBalancer(entities);

            // Act
            var result = balancer.Rebalance();

            // Assert
            Assert.True(result.Success, result.Message);
            Assert.True(result.Entities.All(e => e.Exposure <= e.Capacity));
            Assert.Equal(130, result.Entities.Sum(e => e.Exposure)); // total unchanged
            Assert.Equal(50, result.Entities.First(e => e.EntityId == "A").Exposure); 
        }

        [Fact(DisplayName = "Rebalance should detect already balanced portfolios")]
        public void Rebalance_ShouldHandleAlreadyBalanced()
        {
            // Arrange
            var entities = new List<Entity>
            {
                new("A", 50, 50, 1),
                new("B", 30, 60, 2),
                new("C", 40, 40, 3)
            };

            var balancer = new ExposureBalancer(entities);

            // Act
            var result = balancer.Rebalance();

            // Assert
            Assert.True(result.Success);
            Assert.Contains("No redistribution needed", result.Message);
            Assert.True(result.Entities.All(e => e.Exposure <= e.Capacity));
            Assert.Equal(120, result.Entities.Sum(e => e.Exposure));
        }

        [Fact(DisplayName = "Rebalance should fail gracefully when not possible")]
        public void Rebalance_ShouldFailWhenTotalExposureExceedsCapacity()
        {
            // Arrange
            var entities = new List<Entity>
            {
                new("A", 100, 40, 1),
                new("B", 80, 50, 2)
            };

            var balancer = new ExposureBalancer(entities);

            // Act
            var result = balancer.Rebalance();

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Not possible", result.Message);
        }

        [Fact(DisplayName = "Async rebalance should produce same outcome as sync")]
        public async Task RebalanceAsync_ShouldMatchSyncResults()
        {
            // Arrange
            var entitiesSync = new List<Entity>
            {
                new("A", 70, 50, 1),
                new("B", 30, 60, 2),
                new("C", 20, 40, 3),
                new("D", 10, 20, 4)
            };

            var entitiesAsync = entitiesSync.Select(e =>
                new Entity(e.EntityId, e.Exposure, e.Capacity, e.Priority)).ToList();

            var syncBalancer = new ExposureBalancer(entitiesSync);
            var asyncBalancer = new ExposureBalancer(entitiesAsync);

            // Act
            var syncResult = syncBalancer.Rebalance();
            var asyncResult = await asyncBalancer.RebalanceAsync();

            // Assert
            Assert.True(syncResult.Success);
            Assert.True(asyncResult.Success);
            Assert.Equal(syncResult.Entities.Sum(e => e.Exposure),
                         asyncResult.Entities.Sum(e => e.Exposure));
            Assert.All(asyncResult.Entities, e => Assert.True(e.Exposure <= e.Capacity));
        }
    }
}