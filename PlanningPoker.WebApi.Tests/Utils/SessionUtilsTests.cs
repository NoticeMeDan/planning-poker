using Optional.Unsafe;

namespace PlanningPoker.WebApi.Tests.Utils
{
    using System.Collections.Generic;
    using PlanningPoker.WebApi.Utils;
    using Shared;
    using Xunit;

    public class SessionUtilsTests
    {
        [Fact]
        public void GetCurrentActiveItem_given_empty_list_returns_option_none()
        {
            var input = new List<ItemDTO>();
            var result = SessionUtils.GetCurrentActiveItem(input);

            Assert.False(result.HasValue);
        }

        [Fact]
        public void GetCurrentActiveItem_given_list_that_contains_consensus_returns_option_none()
        {
            var input = new List<ItemDTO>
            {
                new ItemDTO
                {
                    Id = 42,
                    Rounds = new List<RoundDTO>
                    {
                        new RoundDTO
                        {
                            Id = 1,
                            Votes = new List<VoteDTO>
                            {
                                new VoteDTO { Estimate = 13 },
                                new VoteDTO { Estimate = 13 }
                            }
                        }
                    }
                }
            };

            var result = SessionUtils.GetCurrentActiveItem(input);
            Assert.False(result.HasValue);
        }

        [Fact]
        public void GetCurrentActiveItem_given_list_that_contains_no_consensus_returns_option_some()
        {
            var input = new List<ItemDTO>
            {
                new ItemDTO
                {
                    Id = 42,
                    Rounds = new List<RoundDTO>
                    {
                        new RoundDTO
                        {
                            Id = 1,
                            Votes = new List<VoteDTO>
                            {
                                new VoteDTO { Estimate = 13 },
                                new VoteDTO { Estimate = 8 }
                            }
                        }
                    }
                }
            };

            var result = SessionUtils.GetCurrentActiveItem(input);
            Assert.True(result.HasValue);
            Assert.Equal(42, result.ValueOrFailure().Id);
            Assert.Equal(1, result.ValueOrFailure().Rounds.Count);
        }
    }
}
