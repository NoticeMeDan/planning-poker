
using PlanningPoker.Shared;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningPoker.Services
{
    interface ISummaryRepository
    {
        Task<SummaryDTO> CreateAsync(SummaryDTO summary);

        Task<SummaryDTO> FindAsync(int summaryId);

        IQueryable<SummaryDTO> Read();

        Task<bool> UpdateAsync(SummaryDTO summary);

        Task<bool> DeleteAsync(int summaryId);
    }
}
