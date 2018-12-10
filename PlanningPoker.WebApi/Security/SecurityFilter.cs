namespace PlanningPoker.WebApi.Security
{
    using Optional.Unsafe;

    public static class SecurityFilter
    {
        public static bool RequestIsValid(string token, string sessionKey, UserStateManager stateManager)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(sessionKey))
            {
                return false;
            }

            var state = stateManager.GetState(token.Replace("Bearer ", string.Empty));

            if (!state.HasValue)
            {
                return false;
            }

            return state.ValueOrFailure().SessionKey == sessionKey;
        }
    }
}
