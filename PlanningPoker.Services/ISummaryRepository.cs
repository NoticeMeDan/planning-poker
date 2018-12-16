namespace PlanningPoker.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Shared;

    public interface ISummaryRepository
    {
        Task<SummaryDTO> CreateAsync(SummaryCreateUpdateDTO summary);

        Task<SummaryDTO> FindAsync(int summaryId);

        Task<SummaryDTO> FindBySessionIdAsync(int sessionId);

        ICollection<ItemEstimateDTO> BuildItemEstimates(SessionDTO session);

        Task<SummaryDTO> BuildSummary(SessionDTO session);

        IQueryable<SummaryDTO> Read();

        Task<bool> UpdateAsync(SummaryCreateUpdateDTO summary);

        Task<bool> DeleteAsync(int summaryId);
    }
}
