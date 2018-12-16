namespace PlanningPoker.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Shared;

    public interface ISummaryRepository
    {
        Task<SummaryDTO> CreateAsync(SummaryCreateUpdateDTO summary);

        Task<SummaryDTO> FindBySessionIdAsync(int sessionId);

        ICollection<ItemEstimateDTO> BuildItemEstimates(SessionDTO session);

        Task<SummaryDTO> BuildSummary(SessionDTO session);

        Task<bool> DeleteAsync(int summaryId);
    }
}
