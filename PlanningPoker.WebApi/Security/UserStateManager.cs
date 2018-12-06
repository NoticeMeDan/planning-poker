namespace PlanningPoker.WebApi.Security
{
    using System;
    using Microsoft.Extensions.Caching.Memory;
    using Optional;

    public class UserStateManager
    {
        private readonly IMemoryCache cache;

        public UserStateManager(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public string CreateState(int userId)
        {
            var token = Guid.NewGuid().ToString();
            var userState = new UserState { Id = userId };

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(1));

            this.cache.Set(token, userState, cacheOptions);

            return token;
        }

        public Option<UserState> GetState(string token)
        {
            return this.cache.Get(token)
                .SomeNotNull()
                .Map(obj => obj as UserState);
        }
    }
}
