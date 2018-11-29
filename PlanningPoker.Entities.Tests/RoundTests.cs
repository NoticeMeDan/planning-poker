using System.Collections.Generic;
using Xunit;

namespace PlanningPoker.Entities.Tests
{
    public class RoundTests
    {
        [Fact]
        public void Votes_is_HashSet_of_Vote()
        {
            var round = new Round();

            var votes = round.Votes as HashSet<Vote>;

            Assert.NotNull(votes);
        }
    }
}
