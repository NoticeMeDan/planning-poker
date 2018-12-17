namespace PlanningPoker.App.Models
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Shared;

    public interface ISessionClient
    {
        Task<SessionDTO> CreateAsync(SessionCreateUpdateDTO session);

        Task<SessionDTO> FindAsync(int sessionId);

        Task<bool> UpdateAsync(SessionCreateUpdateDTO session);

        Task<SessionDTO> GetByKeyAsync(string key);

        Task<UserStateResponseDTO> Join(string sessionKey, UserCreateDTO user);

        Task<RoundDTO> NextRoundAsync(string sessionKey);

        Task<RoundDTO> GetCurrentRound(string key);

        Task<ItemDTO> NextItemAsync(string key);

        Task<ICollection<ItemDTO>> GetAllItems(string key);

        Task<ItemDTO> GetCurrentItem(string key);

        Task<bool> Vote(string key, VoteDTO vote);

        Task<bool> ThrowNitpickerCard(string key);

        Task<bool> KickUser(string key, int userId);
    }
}
