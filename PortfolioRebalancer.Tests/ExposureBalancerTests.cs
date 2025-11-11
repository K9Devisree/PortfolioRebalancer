using Moq;
using PortfolioRebalancer.Interface;
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
            var validatorMock = new Mock<IEntityValidator>();
            validatorMock.Setup(v => v.Validate(entities))
                         .Returns(new RebalanceResult(true, "Validation passed", entities));

            var balancer = new ExposureBalancer(entities, validatorMock.Object);

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
            var validatorMock = new Mock<IEntityValidator>();
            validatorMock.Setup(v => v.Validate(entities))
                         .Returns(new RebalanceResult(true, "Validation passed", entities));

            var balancer = new ExposureBalancer(entities, validatorMock.Object);

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
            var validatorMock = new Mock<IEntityValidator>();
            validatorMock.Setup(v => v.Validate(entities))
                         .Returns(new RebalanceResult(true, "Validation passed", entities));

            var balancer = new ExposureBalancer(entities, validatorMock.Object);

            // Act
            var result = balancer.Rebalance();

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Not possible", result.Message);
        }
        
    }
}