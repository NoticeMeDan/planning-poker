namespace PlanningPoker.WebApi.Security
{
    public static class SecurityFilter
    {
        public static bool RequestIsValid(string token, UserStateManager stateManager)
        {
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            return stateManager.GetState(token.Replace("Bearer ", string.Empty)).HasValue;
        }
    }
}
