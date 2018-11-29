using System;
using System.Collections.Generic;

namespace PlanningPoker.App.Models
{
    public class Settings : ISettings
    {
        public Uri BackendUrl => new Uri("http://localhost:5001/");

        public string ClientId => "e1ab0ad7-71d7-47a4-a01a-0d78e2a5cf22";

        public string TenantId => "bea229b6-7a08-4086-b44c-71f57f716bdb";

        public IReadOnlyCollection<string> Scopes => new[]
        {
            "User.Read",
        };

        public string Authority => $"https://login.microsoftonline.com/{TenantId}/v2.0/";
    }
}
