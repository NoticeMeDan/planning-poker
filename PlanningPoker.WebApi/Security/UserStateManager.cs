using System;

namespace PlanningPoker.WebApi.Security
{
    using Microsoft.Extensions.Caching.Memory;
    using Optional;

    public class UserStateManager
    {
        private readonly IMemoryCache _cache;

        public UserStateManager(IMemoryCache cache)
        {
            _cache = cache;
        }

        public string CreateState(int userId)
        {
            var token = Guid.NewGuid().ToString();
            var userState = new UserState { Id = userId };

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(1));

            _cache.Set(token, userState, cacheOptions);

            return token;
        }

        public Option<UserState> GetState(string token)
        {
            return _cache.Get(token)
                .SomeNotNull()
                .Map(obj => obj as UserState);
        }
    }
}
