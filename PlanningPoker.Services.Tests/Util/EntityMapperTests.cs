namespace PlanningPoker.Services.Tests.Util
{
    using System.Collections.Generic;
    using System.Linq;
    using DeepEqual.Syntax;
    using Entities;
    using Shared;
    using Xunit;

    public class EntityMapperTests
    {
        [Fact]
        public void ToItemEntities_returns_Collection_of_equal_size()
        {
            var dtos = this.CreateItemDTOList();

            var result = EntityMapper.ToItemEntities(dtos);

            Assert.Equal(dtos.Count, result.Count);
        }

        [Fact]
        public void ToItemEntities_returns_Collection_of_correct_entities()
        {
            var dtos = this.CreateItemDTOList();

            var result = EntityMapper.ToItemEntities(dtos);

            var firstItem = result.ToList().FirstOrDefault();

            Assert.Equal(1, firstItem.Id);
            Assert.Equal("item 1", firstItem.Title);
            Assert.Equal("item 1", firstItem.Description);
        }

        [Fact]
        public void ToItemDtos_returns_Collection_of_equal_size()
        {
           var entities = this.CreateItemEntityList();

            var result = EntityMapper.ToItemDtos(entities);

            Assert.Equal(entities.Count, result.Count);
        }

        [Fact]
        public void ToItemDtos_returns_Collection_of_correct_dtos()
        {
            var entities = this.CreateItemEntityList();

            var result = EntityMapper.ToItemDtos(entities);

            var firstItem = result.ToList().FirstOrDefault();

            Assert.Equal(1, firstItem.Id);
            Assert.Equal("item 1", firstItem.Title);
            Assert.Equal("item 1", firstItem.Description);
        }

        [Fact]
        public void ToRoundEntities_returns_correct_size()
        {
            var dtos = this.CreateRoundDTOHashSet();

            var result = EntityMapper.ToRoundEntities(dtos);

            Assert.Equal(dtos.Count, result.Count);
        }

        [Fact]
        public void ToRoundEntities_returns_correct_entities()
        {
            var dtos = this.CreateRoundDTOHashSet();

            var entities = this.CreateRoundEntityHashSet();

            var entityVote = entities.ToList().FirstOrDefault()?.Votes.ToList().FirstOrDefault();

            var result = EntityMapper.ToRoundEntities(dtos);

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

            var result = EntityMapper.ToRoundDtos(entities);

            Assert.Equal(entities.Count, result.Count);
        }

        [Fact]
        public void ToRoundDtos_returns_correct_dtos()
        {
            var dtos = this.CreateRoundDTOHashSet();

            var dtoVote = dtos.ToList().FirstOrDefault().Votes.ToList().FirstOrDefault();

            var entities = this.CreateRoundEntityHashSet();

            var result = EntityMapper.ToRoundDtos(entities);

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

            var result = EntityMapper.ToVoteEntities(dtos);

            Assert.Equal(dtos.Count, result.Count);
        }

        [Fact]
        public void ToVoteEntities_returns_correct_entities()
        {
            var dtos = this.CreateVoteDTOHashSet();

            var result = EntityMapper.ToVoteEntities(dtos);

            var firstVote = result.ToList().FirstOrDefault();

            Assert.Equal(1, firstVote.Id);
            Assert.Equal(1, firstVote.UserId);
            Assert.Equal(5, firstVote.Estimate);
        }

        [Fact]
        public void ToVoteDtos_returns_correct_size()
        {
            var entities = this.CreateVoteEntityHashSet();

            var result = EntityMapper.ToVoteDtos(entities);

            Assert.Equal(entities.Count, result.Count);
        }

        [Fact]
        public void ToVoteDtos_returns_correct_dtos()
        {
            var entities = this.CreateVoteEntityHashSet();

            var result = EntityMapper.ToVoteDtos(entities);

            var firstVote = result.ToList().FirstOrDefault();

            Assert.Equal(1, firstVote.Id);
            Assert.Equal(1, firstVote.UserId);
            Assert.Equal(5, firstVote.Estimate);
        }

        [Fact]
        public void ToUserEntities_returns_correct_size()
        {
            var dtos = this.CreateUserDTOHashSet();

            var result = EntityMapper.ToUserEntities(dtos);

            Assert.Equal(dtos.Count, result.Count);
        }

        [Fact]
        public void ToUserEntities_returns_correct_entities()
        {
            var dtos = this.CreateUserDTOHashSet();

            var result = EntityMapper.ToUserEntities(dtos);

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

            var result = EntityMapper.ToUserDtos(entities);

            Assert.Equal(entities.Count, result.Count);
        }

        [Fact]
        public void ToUserDtos_returns_correct_dtos()
        {
            var entities = this.CreateUserEntityHashSet();

            var result = EntityMapper.ToUserDtos(entities);

            var firstUser = result.ToList().FirstOrDefault();

            Assert.Equal(1, firstUser.Id);
            Assert.True(firstUser.IsHost);
            Assert.Equal("dummy@user.com", firstUser.Email);
            Assert.Equal("Dummy", firstUser.Nickname);
        }

        [Fact]
        public void ToItemEstimateEntities_returns_correct_size()
        {
            var dtos = this.CreateItemEstimateDTOList();

            var result = EntityMapper.ToItemEstimateEntities(dtos);

            Assert.Equal(dtos.Count, result.Count);
        }

        [Fact]
        public void ToItemEstimateEntities_returns_correct_entities()
        {
            var dtos = this.CreateItemEstimateDTOList();

            var result = EntityMapper.ToItemEstimateEntities(dtos);

            var firstItemEstimate = result.ToList().FirstOrDefault();

            Assert.Equal(1, firstItemEstimate.Id);
            Assert.Equal("Item 1", firstItemEstimate.ItemTitle);
            Assert.Equal(5, firstItemEstimate.Estimate);
        }

        [Fact]
        public void ToItemEstimateDtos_returns_correct_size()
        {
            var entities = this.CreateItemEstimateEntityList();

            var result = EntityMapper.ToItemEstimateDtos(entities);

            Assert.Equal(entities.Count, result.Count);
        }

        [Fact]
        public void ToItemEstimateDtos_returns_correct_dtos()
        {
            var entities = this.CreateItemEstimateEntityList();

            var result = EntityMapper.ToItemEstimateDtos(entities);

            var firstItemEstimate = result.ToList().FirstOrDefault();

            Assert.Equal(1, firstItemEstimate.Id);
            Assert.Equal("Item 1", firstItemEstimate.ItemTitle);
            Assert.Equal(5, firstItemEstimate.Estimate);
        }

        [Fact]
        public void ToSessionDto_returns_correct_dto()
        {
            var session = this.CreateSession();

            var result = EntityMapper.ToSessionDTO(session);

            Assert.Equal(this.CreateSession().Id, result.Id);
            Assert.Equal(this.CreateSession().SessionKey, result.SessionKey);
            Assert.True(this.CreateItemDTOList().IsDeepEqual(result.Items));
            Assert.True(this.CreateUserDTOHashSet().IsDeepEqual(result.Users));
        }

        private List<Item> CreateItemEntityList()
        {
            return new List<Item>
            {
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

        private List<ItemDTO> CreateItemDTOList()
        {
            return new List<ItemDTO>
            {
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
                        new VoteDTO { Id = 1, UserId = 1, Estimate = 5 }
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
                        new Vote { Id = 1, UserId = 1, Estimate = 5 }
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

        private List<ItemEstimateDTO> CreateItemEstimateDTOList()
        {
            return new List<ItemEstimateDTO>
            {
                new ItemEstimateDTO
                {
                    Id = 1,
                    ItemTitle = "Item 1",
                    Estimate = 5
                }
            };
        }

        private List<ItemEstimate> CreateItemEstimateEntityList()
        {
            return new List<ItemEstimate>
            {
                new ItemEstimate
                {
                    Id = 1,
                    ItemTitle = "Item 1",
                    Estimate = 5
                }
            };
        }

        private Session CreateSession()
        {
            return new Session
            {
                Id = 42,
                Items = this.CreateItemEntityList(),
                SessionKey = "ABC123",
                Users = this.CreateUserEntityHashSet()
            };
        }
    }
}
