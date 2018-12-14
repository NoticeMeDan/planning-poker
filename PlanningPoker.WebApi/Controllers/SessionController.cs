using Optional.Unsafe;

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
            return await this.sessionRepository.CreateAsync(session);
        }

        // POST api/session/{key}/join
        [HttpPost("{key}/join")]
        public async Task<ActionResult<UserStateResponseDTO>> Join(string sessionKey, [FromBody] UserCreateDTO user)
        {
            var session = await this.sessionRepository.FindByKeyAsync(sessionKey);

            if (session == null)
            {
                return this.BadRequest();
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
        [HttpPost("{key}/item/next")]
        public async Task<ActionResult<ItemDTO>> NextItem(string sessionKey)
        {
            var authHeader = this.HttpContext.Request.Headers["PPAuthorization"];
            if (!SecurityFilter.RequestIsValid(authHeader, sessionKey, this.userStateManager))
            {
                return this.Unauthorized();
            }

            var session = await this.sessionRepository.FindByKeyAsync(sessionKey);

            if (session == null)
            {
                return this.BadRequest();
            }

            var nextItem = session.Items.FirstOrDefault(item => item.Rounds.Count == 0);

            // If there are no more rounds, the session is complete
            // TODO: Generate summary when this happens
            if (nextItem == default(ItemDTO))
            {
                return this.NotFound();
            }

            nextItem.Rounds.Add(new RoundDTO { Votes = new List<VoteDTO>()});
            session.Items[session.Items.FindIndex(item => item.Id == nextItem.Id)] = nextItem;

            await this.sessionRepository.UpdateAsync(EntityMapper.ToSessionCreateUpdateDTO(session));

            return nextItem;
        }

        [HttpPost("{key}/item")]
        public async Task<ActionResult<ItemDTO>> GetCurrentItem(string sessionKey)
        {
            var authHeader = this.HttpContext.Request.Headers["PPAuthorization"];
            if (!SecurityFilter.RequestIsValid(authHeader, sessionKey, this.userStateManager))
            {
                return this.Unauthorized();
            }

            var session = await this.sessionRepository.FindByKeyAsync(sessionKey);

            if (session == null)
            {
                return this.BadRequest();
            }

            var currentItem = SessionUtils.GetCurrentActiveItem(session.Items);

            if (!currentItem.HasValue)
            {
                return this.NotFound();
            }

            return currentItem.ValueOrDefault();
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
                return this.BadRequest();
            }

            throw new System.NotImplementedException();
        }

        public Task<ActionResult<RoundDTO>> GetCurrentRound(string sessionKey)
        {
            throw new System.NotImplementedException();
        }

        public Task<ActionResult<ICollection<ItemDTO>>> GetAllItems(string sessionKey)
        {
            throw new System.NotImplementedException();
        }

        public Task<ActionResult> Vote(string sessionKey, VoteDTO vote)
        {
            throw new System.NotImplementedException();
        }

        public Task<ActionResult> ThrowNitpickerCard(string sessionKey)
        {
            throw new System.NotImplementedException();
        }

        public Task<ActionResult> KickUser(string sessionKey, int userId)
        {
            throw new System.NotImplementedException();
        }
    }
}
