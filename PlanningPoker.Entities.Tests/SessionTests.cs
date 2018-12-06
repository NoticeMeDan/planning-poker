namespace PlanningPoker.Entities.Tests
{
    using System.Collections.Generic;
    using Xunit;

    public class SessionTests
    {
        [Fact]
        public void Items_is_HashSet_of_Item()
        {
            var session = new Session();

            var items = session.Items as HashSet<Item>;

            Assert.NotNull(items);
        }

        [Fact]
        public void Users_is_HashSet_of_User()
        {
            var session = new Session();

            var users = session.Users as HashSet<User>;

            Assert.NotNull(users);
        }
    }
}
