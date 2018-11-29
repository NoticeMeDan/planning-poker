using System;

namespace PlanningPoker.App.Models
{
    public class Settings : ISettings
    {
        //TODO: Replace with *.azurewebsites.net url after deploying backend to Azure
        public Uri BackendUrl => new Uri("https://localhost:5001/");

        public string ClientId => "490c5823-af8d-4f60-a6cd-30ef581f6abf";

        public string TenantId => "b461d90e-0c15-44ec-adc2-51d14f9f5731";

        public string Authority => $"https://login.microsoftonline.com/{TenantId}/v2.0/";
    }
}
