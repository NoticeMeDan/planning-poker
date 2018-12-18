using PlanningPoker.WebApi.Security;

namespace PlanningPoker.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Shared;

    public interface ISessionController
    {
        Task<ActionResult<SessionDTO>> GetByKey(string sessionKey);

        Task<ActionResult<SessionDTO>> Create(SessionCreateUpdateDTO session);

        Task<ActionResult<UserStateResponseDTO>> Join(string sessionKey, UserCreateDTO user);

        Task<ActionResult<RoundDTO>> NextRound(string authToken, string sessionKey);

        Task<ActionResult<RoundDTO>> GetCurrentRound(string authToken, string sessionKey);

        Task<ActionResult<ItemDTO>> NextItem(string authToken, string sessionKey);

        Task<ActionResult<ICollection<ItemDTO>>> GetAllItems(string authToken, string sessionKey);

        Task<ActionResult<ItemDTO>> GetCurrentItem(string authToken, string sessionKey);

        Task<ActionResult> Vote(string authToken, string sessionKey, VoteCreateUpdateDTO vote);

        Task<ActionResult> ThrowNitpickerCard(string authToken, string sessionKey);

        Task<ActionResult> KickUser(string authToken, string sessionKey, int userId);

        ActionResult<UserState> WhoAmI(string authToken, string sessionKey);
    }
}
