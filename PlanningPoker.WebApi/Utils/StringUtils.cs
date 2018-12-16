namespace PlanningPoker.WebApi.Utils
{
    using System;
    using System.Linq;

    public static class StringUtils
    {
        public static string RandomSessionKey()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 7)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
