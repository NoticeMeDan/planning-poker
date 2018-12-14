using System.Collections.Generic;

namespace PlanningPoker.WebApi.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Shared;

    public interface ISessionController
    {
        Task<ActionResult<SessionDTO>> GetByKey(string key);

        Task<ActionResult<SessionDTO>> Create(SessionCreateUpdateDTO session);

        Task<ActionResult<UserStateResponseDTO>> Join(string sessionKey, UserCreateDTO user);

        Task<ActionResult<RoundDTO>> NextRound(string sessionKey);

        Task<ActionResult<RoundDTO>> GetCurrentRound(string key);

        Task<ActionResult<ItemDTO>> NextItem(string key);

        Task<ActionResult<ICollection<ItemDTO>>> GetAllItems(string key);

        Task<ActionResult<ItemDTO>> GetCurrentItem(string key);

        Task<ActionResult> Vote(string key, VoteDTO vote);

        Task<ActionResult> ThrowNitpickerCard(string key);

        Task<ActionResult> KickUser(string key, int userId);
    }
}
