namespace PlanningPoker.WebApi.Tests.Security
{
    using Microsoft.Extensions.Caching.Memory;
    using PlanningPoker.WebApi.Security;
    using Xunit;
    using System;
    using Optional;
    using Optional.Unsafe;

    public class UserStateManagerTests
    {
        [Fact]
        public void CreateState_given_integer_returns_valid_guid_token()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var stateManager = new UserStateManager(cache);

            var token = stateManager.CreateState(1);
            Assert.True(Guid.TryParse(token, out _));
        }

        [Fact]
        public void GetState_given_valid_token_returns_Option_Some()
        {
            var result = GetTestUserState();
            Assert.True(result.HasValue);
        }

        [Fact]
        public void GetState_given_valid_token_returns_Option_Some_with_state()
        {
            var result = GetTestUserState();
            Assert.Equal(1, result.ValueOrFailure().Id);
        }

        [Fact]
        public void GetState_given_invalid_token_returns_Option_None()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var stateManager = new UserStateManager(cache);

            stateManager.CreateState(1);
            var result = stateManager.GetState("invalidToken");
            Assert.False(result.HasValue);
        }

        private static Option<UserState> GetTestUserState()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var stateManager = new UserStateManager(cache);

            var token = stateManager.CreateState(1);
            return stateManager.GetState(token);
        }
    }
}
