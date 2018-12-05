namespace PlanningPoker.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Shared;
    using Security;

    [Route("api/[controller]")]
    [ApiController]
    public class TokenTestController : ControllerBase
    {
        private readonly UserStateManager userStateManager;

        public TokenTestController(IMemoryCache cache)
        {
            this.userStateManager = new UserStateManager(cache);
        }

        [HttpPost]
        public ActionResult<UserStateResponseDTO> Register([FromBody] UserDTO user)
        {
            return new UserStateResponseDTO { Token = this.userStateManager.CreateState(user.Id) };
        }

        [HttpGet]
        public ActionResult<string> SomethingAuthenticated()
        {
            return SecurityFilter.RequestIsValid(HttpContext.Request.Headers["Authorization"], this.userStateManager)
                ? "Roger doger"
                : "Piss off";
        }
    }
}
