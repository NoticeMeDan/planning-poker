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
            var dtos = this.CreateItemDTOHashSet();
            
            var result = CollectionHandler.ToItemEntities(dtos);

            Assert.Equal(dtos.Count, result.Count);
        }

        [Fact]
        public void ToItemEntities_returns_Collection_of_correct_entities()
        {
            var dtos = this.CreateItemDTOHashSet();

            var entities = this.CreateItemEntityHashSet();

            var result = CollectionHandler.ToItemEntities(dtos);

            var firstItem = result.ToList().FirstOrDefault();

            Assert.Equal(1, firstItem.Id);
            Assert.Equal("item 1", firstItem.Title);
            Assert.Equal("item 1", firstItem.Description);
        }

        [Fact]
        public void ToItemDtos_returns_Collection_of_equal_size()
        {
           var entities = this.CreateItemEntityHashSet();

            var result = CollectionHandler.ToItemDtos(entities);

            Assert.Equal(entities.Count, result.Count);
        }

        [Fact]
        public void ToItemDtos_returns_Collection_of_correct_dtos()
        {
            var dtos = this.CreateItemDTOHashSet();

            var entities = this.CreateItemEntityHashSet();

            var result = CollectionHandler.ToItemDtos(entities);

            var firstItem = result.ToList().FirstOrDefault();

            Assert.Equal(1, firstItem.Id);
            Assert.Equal("item 1", firstItem.Title);
            Assert.Equal("item 1", firstItem.Description);
        }

        [Fact]
        public void ToRoundEntities_returns_correct_size()
        {
            var dtos = this.CreateRoundDTOHashSet();

            var result = CollectionHandler.ToRoundEntities(dtos);

            Assert.Equal(dtos.Count, result.Count);
        }

        [Fact]
        public void ToRoundEntities_returns_correct_entities()
        {
            var dtos = this.CreateRoundDTOHashSet();

            var entities = this.CreateRoundEntityHashSet();

            var entityVote = entities.ToList().FirstOrDefault().Votes.ToList().FirstOrDefault();

            var result = CollectionHandler.ToRoundEntities(dtos);

            var firstRound = result.ToList().FirstOrDefault();

            var roundVote = firstRound.Votes.ToList().FirstOrDefault();

            Assert.Equal(1, firstRound.Id);
            Assert.Equal(entityVote.Estimate, roundVote.Estimate);
            Assert.Equal(entityVote.UserId, roundVote.UserId);
        }

        [Fact]
        public void ToRoundDtos_returns_correct_size()
        {
            var entities = this.CreateRoundEntityHashSet();

            var result = CollectionHandler.ToRoundDtos(entities);

            Assert.Equal(entities.Count, result.Count);
        }


        [Fact]
        public void ToRoundDtos_returns_correct_dtos()
        {
            var dtos = this.CreateRoundDTOHashSet();

            var dtoVote = dtos.ToList().FirstOrDefault().Votes.ToList().FirstOrDefault();

            var entities = this.CreateRoundEntityHashSet();

            var result = CollectionHandler.ToRoundDtos(entities);

            var firstRound = result.ToList().FirstOrDefault();

            var roundVote = firstRound.Votes.ToList().FirstOrDefault();

            Assert.Equal(1, firstRound.Id);
            Assert.Equal(dtoVote.Estimate, roundVote.Estimate);
            Assert.Equal(dtoVote.UserId, roundVote.UserId);
        }

        private HashSet<Item> CreateItemEntityHashSet()
        {
            return new HashSet<Item> {
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
        }

        private HashSet<ItemDTO> CreateItemDTOHashSet()
        {
            return new HashSet<ItemDTO> {
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
        }

        private HashSet<RoundDTO> CreateRoundDTOHashSet()
        {
            return new HashSet<RoundDTO>
            {
                new RoundDTO
                {
                    Id = 1,
                    Votes = new HashSet<VoteDTO>
                    {
                        new VoteDTO { Id = 1, UserId = 1, Estimate = 5}
                    }
                }
            };
        }

        private HashSet<Round> CreateRoundEntityHashSet()
        {
            return new HashSet<Round>
            {
                new Round
                {
                    Id = 1,
                    Votes = new HashSet<Vote>
                    {
                        new Vote { Id = 1, UserId = 1, Estimate = 5}
                    }
                }
            };
        }
    }
}
