namespace PlanningPoker.Entities.Tests
{
    using System.Collections.Generic;
    using Xunit;

    public class ItemTests
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
