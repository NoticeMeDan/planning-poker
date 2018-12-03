namespace PlanningPoker.Services.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using PlanningPoker.Entities;
    using PlanningPoker.Shared;
    using Xunit;

    public class CollectionHandlerTests
    {
        [Fact]
        public void ToItemEntities_returns_Collection_of_equal_size()
        {
            var dtos = new HashSet<ItemDTO> {
                new ItemDTO
                {
                    Id = 1,
                    Title = "item 1",
                    Description = "item 1",
                    Rounds = new HashSet<RoundDTO>()
                },
                new ItemDTO
                {
                    Id = 2,
                    Title = "item 2",
                    Description = "item 2",
                    Rounds = new HashSet<RoundDTO>()
                }
            };

            var entities = new HashSet<Item> {
                new Item
                {
                    Id = 1,
                    Title = "item 1",
                    Description = "item 1",
                    Rounds = new HashSet<Round>()
                },
                new Item
                {
                    Id = 2,
                    Title = "item 2",
                    Description = "item 2",
                    Rounds = new HashSet<Round>()
                }
            };

            var result = CollectionHandler.ToItemEntities(dtos);

            Assert.Equal(entities.Count, result.Count);
        }

        [Fact]
        public void ToItemEntities_returns_Collection_of_correct_entities()
        {
            var dtos = new HashSet<ItemDTO> {
                new ItemDTO
                {
                    Id = 1,
                    Title = "item 1",
                    Description = "item 1",
                    Rounds = new HashSet<RoundDTO>()
                },
                new ItemDTO
                {
                    Id = 2,
                    Title = "item 2",
                    Description = "item 2",
                    Rounds = new HashSet<RoundDTO>()
                }
            };

            var entities = new HashSet<Item> {
                new Item
                {
                    Id = 1,
                    Title = "item 1",
                    Description = "item 1",
                    Rounds = new HashSet<Round>()
                },
                new Item
                {
                    Id = 2,
                    Title = "item 2",
                    Description = "item 2",
                    Rounds = new HashSet<Round>()
                }
            };

            var result = CollectionHandler.ToItemEntities(dtos);

            var firstItem = result.ToList().FirstOrDefault();

            Assert.Equal(1, firstItem.Id);
            Assert.Equal("item 2", firstItem.Title);
            Assert.Equal("item 2", firstItem.Description);
        }
    }
}
