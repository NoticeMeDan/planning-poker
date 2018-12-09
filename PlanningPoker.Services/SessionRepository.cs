namespace PlanningPoker.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PlanningPoker.Entities;
    using PlanningPoker.Shared;

    public class SessionRepository : ISessionRepository
    {
        private readonly IPlanningPokerContext context;

        public SessionRepository(IPlanningPokerContext planningPokerContext)
        {
            this.context = planningPokerContext;
        }

        public async Task<SessionDTO> CreateAsync(SessionCreateUpdateDTO session)
        {
            var entity = new Session
            {
                Items = EntityMapper.ToItemEntities(session.Items),
                Users = EntityMapper.ToUserEntities(session.Users),
                SessionKey = session.SessionKey
            };

            this.context.Sessions.Add(entity);
            this.context.SaveChanges();

            return await this.FindAsync(entity.Id);
        }

        public async Task<bool> DeleteAsync(int sessionID)
        {
            var entity = await this.context.Sessions.FindAsync(sessionID);

            if (entity == null)
            {
                return false;
            }

            this.context.Sessions.Remove(entity);
            this.context.SaveChanges();

            return true;
        }

        public async Task<SessionDTO> FindAsync(int sessionId)
        {
            return await this.context.Sessions
                .Where(s => s.Id == sessionId)
                .Select(s => EntityMapper.ToSessionDTO(s))
                .FirstAsync();
        }

        public async Task<SessionDTO> FindAsyncByKey(string sessionKey)
        {
            return await this.context.Sessions
                .Where(s => s.SessionKey == sessionKey)
                .Select(s => EntityMapper.ToSessionDTO(s))
                .FirstOrDefaultAsync();
        }

        public IQueryable<SessionDTO> Read()
        {
            var entities = this.context.Sessions
                .Select(s => new SessionDTO
                {
                    Id = s.Id,
                    Items = EntityMapper.ToItemDtos(s.Items),
                    Users = EntityMapper.ToUserDtos(s.Users),
                    SessionKey = s.SessionKey
                });

            return entities;
        }

        public async Task<bool> UpdateAsync(SessionCreateUpdateDTO session)
        {
            var entity = await this.context.Sessions.FindAsync(session.Id);

            if (entity == null)
            {
                return false;
            }

            entity.Items = EntityMapper.ToItemEntities(session.Items);
            entity.Users = EntityMapper.ToUserEntities(session.Users);
            entity.SessionKey = session.SessionKey;

            this.context.SaveChanges();

            return true;
        }
    }
}
