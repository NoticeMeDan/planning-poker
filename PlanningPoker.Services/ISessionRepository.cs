namespace PlanningPoker.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using PlanningPoker.Shared;

    public interface ISessionRepository
    {
        Task<SessionDTO> CreateAsync(SessionCreateUpdateDTO session);

        Task<SessionDTO> FindAsync(int sessionId);

        Task<SessionDTO> FindAsyncByKey(string sessionKey);

        IQueryable<SessionDTO> Read();

        Task<bool> UpdateAsync(SessionCreateUpdateDTO session);

        Task<bool> DeleteAsync(int sessionID);
    }
}
