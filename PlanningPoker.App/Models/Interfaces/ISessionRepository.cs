namespace PlanningPoker.App.Models.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PlanningPoker.Shared;

    public interface ISessionRepository
    {
        Task<SessionDTO> CreateAsync(SessionCreateUpdateDTO session);

        Task<SessionDTO> FindAsync(int sessionId);

        Task<IEnumerable<SessionDTO>> ReadAsync();

        Task<bool> UpdateAsync(SessionCreateUpdateDTO session);

        Task<bool> DeleteAsync(int sessionId);
    }
}
