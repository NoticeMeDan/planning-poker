namespace PlanningPoker.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using PlanningPoker.Entities;
    using PlanningPoker.Shared;

    public class UserRepository : IUserRepository
    {
        private readonly IPlanningPokerContext context;

        public UserRepository(IPlanningPokerContext planningPokerContext)
        {
            this.context = planningPokerContext;
        }

        public async Task<UserDTO> CreateAsync(UserCreateUpdateDTO user)
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

        public Task<UserDTO> FindAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UserDTO> Read()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(UserCreateUpdateDTO user)
        {
            throw new NotImplementedException();
        }
    }
}
