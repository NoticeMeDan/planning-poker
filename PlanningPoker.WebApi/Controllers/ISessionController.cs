using System.Collections.Generic;

namespace PlanningPoker.WebApi.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Shared;

    public interface ISessionController
    {
        Task<ActionResult<SessionDTO>> GetByKey(string sessionKey);

        Task<ActionResult<SessionDTO>> Create(SessionCreateUpdateDTO session);

        Task<ActionResult<UserStateResponseDTO>> Join(string sessionKey, UserCreateDTO user);

        Task<ActionResult<RoundDTO>> NextRound(string sessionKey);

        Task<ActionResult<RoundDTO>> GetCurrentRound(string sessionKey);

        Task<ActionResult<ItemDTO>> NextItem(string sessionKey);

        Task<ActionResult<ICollection<ItemDTO>>> GetAllItems(string sessionKey);

        Task<ActionResult<ItemDTO>> GetCurrentItem(string sessionKey);

        Task<ActionResult> Vote(string sessionKey, VoteDTO vote);

        Task<ActionResult> ThrowNitpickerCard(string sessionKey);

        Task<ActionResult> KickUser(string sessionKey, int userId);
    }
}
