using System.Linq;
using System.Threading.Tasks;
using PlanningPoker.Shared;

namespace PlanningPoker.Services
{
    public interface ISessionRepository
    {
        Task<SessionDTO> CreateAsync(SessionDTO session);

        Task<SessionDTO> FindAsync(int sessionId);

        IQueryable<SessionDTO> Read();

        Task<bool> UpdateAsync(SessionDTO session);

        Task<bool> DeleteAsync(int sessionID);
    }
}
