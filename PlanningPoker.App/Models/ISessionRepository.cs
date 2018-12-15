namespace PlanningPoker.App.Models
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Shared;

    public interface ISessionRepository
    {
        Task<SessionDTO> CreateAsync(SessionCreateUpdateDTO session);

        Task<SessionDTO> FindAsync(int sessionId);

        Task<IEnumerable<SessionDTO>> ReadAsync();

        Task<bool> UpdateAsync(SessionCreateUpdateDTO session);
    }
}
