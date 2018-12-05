namespace PlanningPoker.WebApi.Security
{
    public static class SecurityFilter
    {
        public static bool RequestIsValid(string token, UserStateManager stateManager)
        {
            return stateManager.GetState(token.Replace("Bearer ", "")).HasValue;
        }
    }
}
