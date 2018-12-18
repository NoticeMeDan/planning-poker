namespace PlanningPoker.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Microsoft.EntityFrameworkCore;
    using Shared;
    using Util;

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
                .Include("Items.Rounds.Votes")
                .Include(u => u.Users)
                .Select(s => EntityMapper.ToSessionDto(s))
                .FirstAsync();
        }

        public async Task<SessionDTO> FindByKeyAsync(string sessionKey)
        {
            return await this.context.Sessions
                .Where(s => s.SessionKey == sessionKey)
                .Include("Items.Rounds.Votes")
                .Include(u => u.Users)
                .Select(s => EntityMapper.ToSessionDto(s))
                .FirstOrDefaultAsync();
        }

        public IQueryable<SessionDTO> Read()
        {
            var entities = this.context.Sessions
                .Include("Items.Rounds.Votes")
                .Include(u => u.Users)
                .Select(s => new SessionDTO
                {
                    Id = s.Id,
                    Items = EntityMapper.ToItemDtos(s.Items),
                    Users = EntityMapper.ToUserDtos(s.Users),
                    SessionKey = s.SessionKey
                });

            return entities;
        }

        public async Task<UserDTO> AddUserToSession(UserCreateDTO user, int sessionId)
        {
            var newUser = new User { Email = user.Email, IsHost = user.IsHost, SessionId = sessionId, Nickname = user.Nickname };
            this.context.Users.Add(newUser);
            this.context.SaveChanges();

            return new UserDTO
            {
                Id = newUser.Id,
                Email = newUser.Email,
                IsHost = newUser.IsHost,
                Nickname = newUser.Nickname
            };
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

            this.context.SaveChanges();

            return true;
        }
    }
}
