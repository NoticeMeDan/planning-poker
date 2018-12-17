namespace PlanningPoker.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Optional.Unsafe;
    using Security;
    using Services;
    using Services.Util;
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
#if DEBUG
#else
    [Authorize]
#endif
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
            return created;
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
                return this.BadRequest();
            }

            var createdUser = await this.userRepository.CreateAsync(user);

            session.Users.Add(createdUser);
            await this.sessionRepository.UpdateAsync(EntityMapper.ToSessionCreateUpdateDto(session));

            return new UserStateResponseDTO { Token = this.userStateManager.CreateState(createdUser.Id, sessionKey) };
        }

        public Task<ActionResult<RoundDTO>> NextRound(string authToken, string sessionKey)
        {
            throw new System.NotImplementedException();
        }

        public Task<ActionResult<RoundDTO>> GetCurrentRound(string authToken, string sessionKey)
        {
            throw new System.NotImplementedException();
        }

        [Authorize]
        [HttpPost("{key}/item/next")]
        public async Task<ActionResult<ItemDTO>> NextItem([FromHeader(Name = "PPAuthorization")] string authToken, string sessionKey)
        {
            if (!SecurityFilter.RequestIsValid(authToken, sessionKey, this.userStateManager))
            {
                return this.Unauthorized();
            }

            var session = await this.sessionRepository.FindByKeyAsync(sessionKey);

            if (session == null)
            {
                return this.NotFound();
            }

            var nextItem = session.Items.FirstOrDefault(item => item.Rounds.Count == 0);

            // If there are no more items without any rounds, the session is complete
            // TODO: Generate summary when this happens
            if (nextItem == default(ItemDTO))
            {
                return this.BadRequest();
            }

            nextItem.Rounds.Add(new RoundDTO { Votes = new List<VoteDTO>() });
            session.Items[session.Items.FindIndex(item => item.Id == nextItem.Id)] = nextItem;

            await this.sessionRepository.UpdateAsync(EntityMapper.ToSessionCreateUpdateDto(session));

            return nextItem;
        }

        public async Task<ActionResult<ICollection<ItemDTO>>> GetAllItems(string authToken, string sessionKey)
        {
            if (!SecurityFilter.RequestIsValid(authToken, sessionKey, this.userStateManager))
            {
                return this.Unauthorized();
            }

            var session = await this.sessionRepository.FindByKeyAsync(sessionKey);

            if (session == null)
            {
                return this.NotFound();
            }

            return session.Items;
        }

        [HttpPost("{key}/item")]
        public async Task<ActionResult<ItemDTO>> GetCurrentItem(string authToken, string sessionKey)
        {
            /*if (!SecurityFilter.RequestIsValid(authToken, sessionKey, this.userStateManager))
            {
                return this.Unauthorized();
            }*/

            var session = await this.sessionRepository.FindByKeyAsync(sessionKey);

            if (session == null)
            {
                return this.NotFound();
            }

            var currentItem = SessionUtils.GetCurrentActiveItem(session.Items, session.Users.Count);

            if (!currentItem.HasValue)
            {
                return this.BadRequest();
            }

            return currentItem.ValueOrDefault();
        }

        public Task<ActionResult> Vote(string authToken, string sessionKey, VoteDTO vote)
        {
            throw new System.NotImplementedException();
        }

        public Task<ActionResult> ThrowNitpickerCard(string authToken, string sessionKey)
        {
            throw new System.NotImplementedException();
        }

        public Task<ActionResult> KickUser(string authToken, string sessionKey, int userId)
        {
            throw new System.NotImplementedException();
        }
    }
}
