using PlanningPoker.Shared;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningPoker.Services
{
    public interface IUserRepository
    {
        Task<UserDTO> CreateAsync(UserDTO user);

        Task<UserDTO> FindAsync(int userId);

        IQueryable<UserDTO> Read();

        Task<bool> UpdateAsync(UserDTO user);

        Task<bool> DeleteAsync(int userId);
    }
}
