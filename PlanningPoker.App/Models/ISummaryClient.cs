namespace PlanningPoker.App.Models
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Shared;

    public interface ISummaryClient
    {
        Task<SummaryDTO> FindBySessionIdAsync(int sessionId);
    }
}
