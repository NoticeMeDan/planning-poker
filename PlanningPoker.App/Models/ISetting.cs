using System;

namespace PlanningPoker.App.Models
{
    public interface ISettings
    {
        Uri BackendUrl { get; }
        string ClientId { get; }
        string TenantId { get; }
    }
}
