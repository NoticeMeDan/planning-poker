namespace PlanningPoker.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using PlanningPoker.Shared;

    public interface IUserRepository
    {
        Task<UserDTO> CreateAsync(UserCreateUpdateDTO user);

        Task<UserDTO> FindAsync(int userId);

        IQueryable<UserDTO> Read();

        Task<bool> UpdateAsync(UserCreateUpdateDTO user);

        Task<bool> DeleteAsync(int userId);
    }
}
