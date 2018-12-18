namespace PlanningPoker.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using Shared;

    public interface ISessionRepository
    {
        Task<SessionDTO> CreateAsync(SessionCreateUpdateDTO session);

        Task<SessionDTO> FindAsync(int sessionId);

        Task<SessionDTO> FindByKeyAsync(string sessionKey);

        IQueryable<SessionDTO> Read();

        Task<bool> UpdateAsync(SessionCreateUpdateDTO session);

        Task<UserDTO> AddUserToSession(UserCreateDTO user, int sessionId);

        Task<bool> DeleteAsync(int sessionID);
    }
}
