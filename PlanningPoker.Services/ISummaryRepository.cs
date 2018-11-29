
using PlanningPoker.Shared;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningPoker.Services
{
    public interface ISummaryRepository
    {
        Task<SummaryDTO> CreateAsync(SummaryCreateUpdateDTO summary);

        Task<SummaryDTO> FindAsync(int summaryId);

        IQueryable<SummaryDTO> Read();

        Task<bool> UpdateAsync(SummaryCreateUpdateDTO summary);

        Task<bool> DeleteAsync(int summaryId);
    }
}
