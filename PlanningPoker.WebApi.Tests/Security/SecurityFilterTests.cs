namespace PlanningPoker.WebApi.Tests.Security
{
    using Microsoft.Extensions.Caching.Memory;
    using PlanningPoker.WebApi.Security;
    using Xunit;

    public class SecurityFilterTests
    {
        [Fact]
        public void RequestIsValid_given_valid_token_returns_true()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var stateManager = new UserStateManager(cache);

            var token = stateManager.CreateState(1, "ABC1234");
            Assert.True(SecurityFilter.RequestIsValid(token, "ABC1234", stateManager));
        }

        [Fact]
        public void RequestIsValid_given_invalid_token_returns_true()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var stateManager = new UserStateManager(cache);

            Assert.False(SecurityFilter.RequestIsValid("invalidtoken123123", "ABC1234", stateManager));
        }

        [Fact]
        public void RequestIsValid_given_null_token_returns_false()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var stateManager = new UserStateManager(cache);

            Assert.False(SecurityFilter.RequestIsValid(null, null, stateManager));
        }
    }
}
