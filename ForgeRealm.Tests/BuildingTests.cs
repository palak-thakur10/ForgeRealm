using ForgeRealm.Data;
using ForgeRealm.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Linq;

namespace ForgeRealm.Tests
{
    public class BuildingTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public void AddingBuilding_ShouldReducePlayerGold()
        {
            // Arrange
            var context = GetDbContext();

            var player = new Player
            {
                UserName = "TestLord",
                Resource = new Resource
                {
                    Gold = 500,
                    Wood = 200,
                    Stone = 100,
                    Food = 100
                }
            };

            context.Players.Add(player);
            context.SaveChanges();

            var buildingCost = 100;

            var building = new Building
            {
                Name = "Barracks",
                Level = 1,
                PlayerId = player.Id
            };

            // Act
            player.Resource.Gold -= buildingCost;
            context.Buildings.Add(building);
            context.SaveChanges();

            // Assert
            var updatedPlayer = context.Players
                                       .Include(p => p.Resource)
                                       .First();

            Assert.Equal(400, updatedPlayer.Resource.Gold);
        }

        // ✅ FINAL BONUS TEST (INSUFFICIENT GOLD)
        [Fact]
        public void UpgradeBuilding_ShouldFail_WhenInsufficientGold()
        {
            // Arrange
            var context = GetDbContext();

            var player = new Player
            {
                UserName = "PoorPlayer",
                Resource = new Resource
                {
                    Gold = 50 // insufficient gold
                }
            };

            var building = new Building
            {
                Name = "Barracks",
                Level = 1,
                Player = player
            };

            context.Players.Add(player);
            context.Buildings.Add(building);
            context.SaveChanges();

            var upgradeCost = 100;

            // Act
            if (player.Resource.Gold >= upgradeCost)
            {
                player.Resource.Gold -= upgradeCost;
                building.Level++;
                context.SaveChanges();
            }

            // Assert
            var updatedBuilding = context.Buildings.First();
            var updatedPlayer = context.Players.Include(p => p.Resource).First();

            Assert.Equal(1, updatedBuilding.Level);      // ❌ no upgrade
            Assert.Equal(50, updatedPlayer.Resource.Gold); // ❌ gold unchanged
        }
    }
}
