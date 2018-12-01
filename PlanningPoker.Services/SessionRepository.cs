namespace PlanningPoker.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using PlanningPoker.Entities;
    using PlanningPoker.Shared;

    public class SessionRepository : ISessionRepository
    {
        private readonly IPlanningPokerContext context;

        public SessionRepository(IPlanningPokerContext planningPokerContext)
        {
            this.context = planningPokerContext;
        }

        public Task<SessionDTO> CreateAsync(SessionCreateUpdateDTO session)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteAsync(int sessionID)
        {
            throw new System.NotImplementedException();
        }

        public Task<SessionDTO> FindAsync(int sessionId)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<SessionDTO> Read()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateAsync(SessionCreateUpdateDTO session)
        {
            throw new System.NotImplementedException();
        }
    }
}
