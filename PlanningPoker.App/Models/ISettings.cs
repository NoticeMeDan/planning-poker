namespace PlanningPoker.App.Models
{
    using System;
    using System.Collections.Generic;

    public interface ISettings
    {
        Uri BackendUrl { get; }

        string ClientId { get; }

        string TenantId { get; }

        IReadOnlyCollection<string> Scopes { get; }
    }
}
