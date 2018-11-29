using System.Collections.Generic;
using Xunit;

namespace PlanningPoker.Entities.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Rounds_is_HashSet_of_Round()
        {
            var item = new Item();

            var rounds = item.Rounds as HashSet<Round>;

            Assert.NotNull(rounds);
        }
    }
}
