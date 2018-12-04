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
        private readonly UserStateManager _userStateManager;

        public TokenTestController(IMemoryCache cache)
        {
            _userStateManager = new UserStateManager(cache);
        }

        [HttpPost]
        public ActionResult<string> Register([FromBody] UserDTO user)
        {
            return _userStateManager.CreateState(user.Id);
        }

        [HttpGet]
        public ActionResult<string> SomethingAuthenticated()
        {
            return SecurityFilter.ValidateRequest(HttpContext.Request.Headers["Authorization"], _userStateManager)
                ? "Roger doger"
                : "Piss off";
        }
    }
}
