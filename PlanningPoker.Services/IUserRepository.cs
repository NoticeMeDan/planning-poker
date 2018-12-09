namespace PlanningPoker.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using PlanningPoker.Shared;

    public interface IUserRepository
    {
        Task<UserDTO> CreateAsync(UserCreateDTO user);

        Task<UserDTO> FindAsync(int userId);

        IQueryable<UserDTO> Read();

        Task<bool> UpdateAsync(UserCreateDTO user);

        Task<bool> DeleteAsync(int userId);
    }
}
