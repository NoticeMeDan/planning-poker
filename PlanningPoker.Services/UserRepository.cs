namespace PlanningPoker.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PlanningPoker.Entities;
    using PlanningPoker.Shared;

    public class UserRepository : IUserRepository
    {
        private readonly IPlanningPokerContext context;

        public UserRepository(IPlanningPokerContext planningPokerContext)
        {
            this.context = planningPokerContext;
        }

        public async Task<UserDTO> CreateAsync(UserCreateDTO user)
        {
            var entity = new User
            {
                IsHost = user.IsHost,
                Email = user.Email,
                Nickname = user.Nickname
            };

            this.context.Users.Add(entity);
            this.context.SaveChanges();

            return await this.FindAsync(entity.Id);
        }

        public async Task<bool> DeleteAsync(int userId)
        {
            var entity = await this.context.Users.FindAsync(userId);

            if (entity == null)
            {
                return false;
            }

            this.context.Users.Remove(entity);
            this.context.SaveChanges();

            return true;
        }

        public async Task<UserDTO> FindAsync(int userId)
        {
            var entities = this.context.Users
                .Where(u => u.Id == userId)
                .Select(u => new UserDTO
                {
                    Id = userId,
                    IsHost = u.IsHost,
                    Email = u.Email,
                    Nickname = u.Nickname
                });

            return await entities.FirstOrDefaultAsync();
        }

        public IQueryable<UserDTO> Read()
        {
            var entities = this.context.Users
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    IsHost = u.IsHost,
                    Email = u.Email,
                    Nickname = u.Nickname
                });

            return entities;
        }

        public async Task<bool> UpdateAsync(UserCreateDTO user)
        {
            var entity = await this.context.Users.FindAsync(user.Id);

            if (entity == null)
            {
                return false;
            }

            entity.Id = user.Id;
            entity.IsHost = user.IsHost;
            entity.Email = user.Email;
            entity.Nickname = user.Nickname;

            this.context.SaveChanges();

            return true;
        }
    }
}
