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
                Items = CollectionHandler.ToItemEntities(session.Items),
                Users = CollectionHandler.ToUserEntities(session.Users),
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
            var entities = this.context.Sessions
                .Where(s => s.Id == sessionId)
                .Select(s => new SessionDTO
                {
                    Id = s.Id,
                    Items = CollectionHandler.ToItemDtos(s.Items),
                    Users = CollectionHandler.ToUserDtos(s.Users),
                    SessionKey = s.SessionKey
                });

            return await entities.FirstOrDefaultAsync();
        }

        public IQueryable<SessionDTO> Read()
        {
            var entities = this.context.Sessions
                .Select(s => new SessionDTO
                {
                    Id = s.Id,
                    Items = CollectionHandler.ToItemDtos(s.Items),
                    Users = CollectionHandler.ToUserDtos(s.Users),
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

            entity.Items = CollectionHandler.ToItemEntities(session.Items);
            entity.Users = CollectionHandler.ToUserEntities(session.Users);
            entity.SessionKey = session.SessionKey;

            this.context.SaveChanges();

            return true;
        }
    }
}
