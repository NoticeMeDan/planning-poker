namespace PlanningPoker.App.Models.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PlanningPoker.Shared;

    public interface ISummaryRepository
    {
        Task<SummaryDTO> CreateAsync(SummaryCreateUpdateDTO summary);

        Task<SummaryDTO> FindAsync(int summaryId);

        Task<IEnumerable<SummaryDTO>> ReadAsync();

        Task<bool> UpdateAsync(SummaryCreateUpdateDTO summary);

        Task<bool> DeleteAsync(int summaryId);
    }
}
