namespace PlanningPoker.App.Models
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Shared;

    public interface ISummaryRepository
    {
        Task<SummaryDTO> FindAsync(int summaryId);
    }
}
