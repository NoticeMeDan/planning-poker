using System;
using System.Collections.Generic;

namespace PlanningPoker.App.Models
{
    public interface ISettings
    {
        Uri BackendUrl { get; }
        string ClientId { get; }
        string TenantId { get; }
        IReadOnlyCollection<string> Scopes { get; }
    }
}
