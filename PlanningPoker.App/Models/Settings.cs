namespace PlanningPoker.App.Models
{
    using System;
    using System.Collections.Generic;

    public class Settings : ISettings
    {
        public Uri BackendUrl => new Uri("http://10.0.2.2:5001/");

        public string ClientId => "e1ab0ad7-71d7-47a4-a01a-0d78e2a5cf22";

        public string TenantId => "bea229b6-7a08-4086-b44c-71f57f716bdb";

        public IReadOnlyCollection<string> Scopes => new[]
        {
            "https://ituniversity.onmicrosoft.com/PlanningPoker.WebApi/user_impersonation"
        };

        public string Authority => $"https://login.microsoftonline.com/{this.TenantId}/v2.0/";
    }
}
