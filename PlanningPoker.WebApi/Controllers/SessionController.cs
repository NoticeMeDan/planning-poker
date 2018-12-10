namespace PlanningPoker.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Security;
    using Services;
    using Shared;
    using Utils;

    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase, ISessionController
    {
        private readonly ISessionRepository sessionRepository;
        private readonly IUserRepository userRepository;
        private readonly UserStateManager userStateManager;

        public SessionController(ISessionRepository sessionRepo, IUserRepository userRepo, IMemoryCache cache)
        {
            this.sessionRepository = sessionRepo;
            this.userRepository = userRepo;
            this.userStateManager = new UserStateManager(cache);
        }

        // GET api/session/52A24B
        [HttpGet("{key}")]
        public async Task<ActionResult<SessionDTO>> GetByKey(string key)
        {
            var session = await this.sessionRepository.FindByKeyAsync(key);
             if (session == null)
            {
                return this.NotFound();
            }

            return session;
        }

        // POST api/session
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<SessionDTO>> Create([FromBody] SessionCreateUpdateDTO session)
        {
            var key = string.Empty;
            while (key == string.Empty)
            {
                var randomKey = StringUtils.RandomSessionKey();
                if (await this.sessionRepository.FindByKeyAsync(randomKey) == null)
                {
                    key = randomKey;
                }
            }

            session.SessionKey = key;
            session.Users = new List<UserCreateDTO>();
            var created = await this.sessionRepository.CreateAsync(session);
            return this.CreatedAtAction(nameof(this.GetByKey), new { created.SessionKey }, created);
        }

        // POST api/session/{key}/join
        [HttpPost("{key}/join")]
        public async Task<ActionResult<UserStateResponseDTO>> Join(string sessionKey, [FromBody] UserCreateDTO user)
        {
            var session = await this.sessionRepository.FindByKeyAsync(sessionKey);

            if (session == null)
            {
                return this.NotFound();
            }

            if (session.Users.ToList().Find(u => u.IsHost) != default(UserDTO) && user.IsHost)
            {
                // If joining user IsHost, but session already has a host
                return this.Forbid();
            }

            var createdUser = await this.userRepository.CreateAsync(user);

            session.Users.Add(createdUser);
            await this.sessionRepository.UpdateAsync(EntityMapper.ToSessionCreateUpdateDTO(session));

            return new UserStateResponseDTO { Token = this.userStateManager.CreateState(createdUser.Id, sessionKey) };
        }

        [Authorize]
        public async Task<ActionResult<RoundDTO>> NextRound(string sessionKey)
        {
            var authHeader = this.HttpContext.Request.Headers["PPAuthorization"];
            if (!SecurityFilter.RequestIsValid(authHeader, sessionKey, this.userStateManager))
            {
                return this.Unauthorized();
            }

            var session = await this.sessionRepository.FindByKeyAsync(sessionKey);

            if (session == null)
            {
                return this.NotFound();
            }



            throw new System.NotImplementedException();
        }

        public Task<ActionResult<RoundDTO>> GetCurrentRound(string key)
        {
            throw new System.NotImplementedException();
        }

        public Task<ActionResult<ItemDTO>> NextItem(string key)
        {
            throw new System.NotImplementedException();
        }

        public Task<ActionResult<ItemDTO>> GetCurrentItem(string key)
        {
            throw new System.NotImplementedException();
        }

        public Task<ActionResult> Vote(string key, VoteDTO vote)
        {
            throw new System.NotImplementedException();
        }

        public Task<ActionResult> ThrowNitpickerCard(string key)
        {
            throw new System.NotImplementedException();
        }
    }
}
