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

        public Task<UserDTO> CreateAsync(UserCreateUpdateDTO user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int userId)
        {
            throw new NotImplementedException();
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
