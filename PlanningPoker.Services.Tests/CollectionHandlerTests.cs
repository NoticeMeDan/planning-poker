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

        [Fact]
        public void ToVoteEntities_returns_correct_size()
        {
            var dtos = this.CreateVoteDTOHashSet();

            var result = CollectionHandler.ToVoteEntities(dtos);

            Assert.Equal(dtos.Count, result.Count);
        }

        [Fact]
        public void ToVoteEntities_returns_correct_entities()
        {
            var dtos = this.CreateVoteDTOHashSet();

            var entities = this.CreateVoteEntityHashSet();

            var result = CollectionHandler.ToVoteEntities(dtos);

            var firstVote = result.ToList().FirstOrDefault();

            Assert.Equal(1, firstVote.Id);
            Assert.Equal(1, firstVote.UserId);
            Assert.Equal(5, firstVote.Estimate);
        }

        [Fact]
        public void ToVoteDtos_returns_correct_size()
        {
            var entities = this.CreateVoteEntityHashSet();

            var result = CollectionHandler.ToVoteDtos(entities);

            Assert.Equal(entities.Count, result.Count);
        }

        [Fact]
        public void ToVoteDtos_returns_correct_dtos()
        {
            var dtos = this.CreateVoteDTOHashSet();

            var entities = this.CreateVoteEntityHashSet();

            var result = CollectionHandler.ToVoteDtos(entities);

            var firstVote = result.ToList().FirstOrDefault();

            Assert.Equal(1, firstVote.Id);
            Assert.Equal(1, firstVote.UserId);
            Assert.Equal(5, firstVote.Estimate);
        }

        [Fact]
        public void ToUserEntities_returns_correct_size()
        {
            var dtos = this.CreateUserDTOHashSet();

            var result = CollectionHandler.ToUserEntities(dtos);

            Assert.Equal(dtos.Count, result.Count);
        }

        [Fact]
        public void ToUserEntities_returns_correct_entities()
        {
            var dtos = this.CreateUserDTOHashSet();

            var entities = this.CreateUserEntityHashSet();

            var result = CollectionHandler.ToUserEntities(dtos);

            var firstUser = result.ToList().FirstOrDefault();

            Assert.Equal(1, firstUser.Id);
            Assert.True(firstUser.IsHost);
            Assert.Equal("dummy@user.com", firstUser.Email);
            Assert.Equal("Dummy", firstUser.Nickname);
        }

        [Fact]
        public void ToUserDtos_returns_correct_size()
        {
            var entities = this.CreateUserEntityHashSet();

            var result = CollectionHandler.ToUserDtos(entities);

            Assert.Equal(entities.Count, result.Count);
        }

        [Fact]
        public void ToUserDtos_returns_correct_dtos()
        {
            var dtos = this.CreateUserDTOHashSet();

            var entities = this.CreateUserEntityHashSet();

            var result = CollectionHandler.ToUserDtos(entities);

            var firstUser = result.ToList().FirstOrDefault();

            Assert.Equal(1, firstUser.Id);
            Assert.True(firstUser.IsHost);
            Assert.Equal("dummy@user.com", firstUser.Email);
            Assert.Equal("Dummy", firstUser.Nickname);
        }

        [Fact]
        public void ToItemEstimateEntities_returns_correct_size()
        {
            var dtos = this.CreateItemEstimateDTOHashSet();

            var result = CollectionHandler.ToItemEstimateEntities(dtos);

            Assert.Equal(dtos.Count, result.Count);
        }

        [Fact]
        public void ToItemEstimateEntities_returns_correct_entities()
        {
            var dtos = this.CreateItemEstimateDTOHashSet();

            var entities = this.CreateItemEstimateEntityHashSet();

            var result = CollectionHandler.ToItemEstimateEntities(dtos);

            var firstItemEstimate = result.ToList().FirstOrDefault();

            Assert.Equal(1, firstItemEstimate.Id);
            Assert.Equal("Item 1", firstItemEstimate.ItemTitle);
            Assert.Equal(5, firstItemEstimate.Estimate);
        }

        [Fact]
        public void ToItemEstimateDtos_returns_correct_size()
        {
            var entities = this.CreateItemEstimateEntityHashSet();

            var result = CollectionHandler.ToItemEstimateDtos(entities);

            Assert.Equal(entities.Count, result.Count);
        }

        [Fact]
        public void ToItemEstimateDtos_returns_correct_dtos()
        {
            var dtos = this.CreateItemEstimateDTOHashSet();

            var entities = this.CreateItemEstimateEntityHashSet();

            var result = CollectionHandler.ToItemEstimateDtos(entities);

            var firstItemEstimate = result.ToList().FirstOrDefault();

            Assert.Equal(1, firstItemEstimate.Id);
            Assert.Equal("Item 1", firstItemEstimate.ItemTitle);
            Assert.Equal(5, firstItemEstimate.Estimate);
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

        private HashSet<VoteDTO> CreateVoteDTOHashSet()
        {
            return new HashSet<VoteDTO>
            {
                new VoteDTO
                {
                    Id = 1,
                    UserId = 1,
                    Estimate = 5
                }
            };
        }

        private HashSet<Vote> CreateVoteEntityHashSet()
        {
            return new HashSet<Vote>
            {
                new Vote
                {
                    Id = 1,
                    UserId = 1,
                    Estimate = 5
                }
            };
        }

        private HashSet<UserDTO> CreateUserDTOHashSet()
        {
            return new HashSet<UserDTO>
            {
                new UserDTO
                {
                    Id = 1,
                    IsHost = true,
                    Email = "dummy@user.com",
                    Nickname = "Dummy"
                }
            };
        }

        private HashSet<User> CreateUserEntityHashSet()
        {
            return new HashSet<User>
            {
                new User
                {
                    Id = 1,
                    IsHost = true,
                    Email = "dummy@user.com",
                    Nickname = "Dummy"
                }
            };
        }

        private HashSet<ItemEstimateDTO> CreateItemEstimateDTOHashSet()
        {
            return new HashSet<ItemEstimateDTO>
            {
                new ItemEstimateDTO
                {
                    Id = 1,
                    ItemTitle = "Item 1",
                    Estimate = 5
                }
            };
        }

        private HashSet<ItemEstimate> CreateItemEstimateEntityHashSet()
        {
            return new HashSet<ItemEstimate>
            {
                new ItemEstimate
                {
                    Id = 1,
                    ItemTitle = "Item 1",
                    Estimate = 5
                }
            };
        }
    }
}
