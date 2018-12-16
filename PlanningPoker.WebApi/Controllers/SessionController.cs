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
//        [Authorize]
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
        [HttpPost("{sessionKey}/join")]
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

            return new UserStateResponseDTO {Token = this.userStateManager.CreateState(createdUser.Id, sessionKey)};
        }

        [HttpPost("{sessionKey}/item/round/next")]
        public async Task<ActionResult<RoundDTO>> NextRound([FromHeader(Name = "PPAuthorization")] string authToken,
            string sessionKey)
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

            var currentItem = SessionUtils.GetCurrentActiveItem(session.Items, session.Users.Count);

            if (!currentItem.HasValue)
            {
                return this.BadRequest();
            }

            var updatedItem = currentItem.ValueOrDefault();
            var newRound = new RoundDTO {Votes = new List<VoteDTO>()};

            updatedItem.Rounds.Add(newRound);
            session.Items[session.Items.FindIndex(item => item.Id == updatedItem.Id)] = updatedItem;

            await this.sessionRepository.UpdateAsync(EntityMapper.ToSessionCreateUpdateDto(session));

            return newRound;
        }

        [HttpGet("{sessionKey}/item/round")]
        public async Task<ActionResult<RoundDTO>> GetCurrentRound(
            [FromHeader(Name = "PPAuthorization")] string authToken, string sessionKey)
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

            var currentItem = SessionUtils.GetCurrentActiveItem(session.Items, session.Users.Count);

            if (!currentItem.HasValue)
            {
                return this.BadRequest();
            }

            return currentItem.ValueOrDefault().Rounds.Last();
        }

        [Authorize]
        [HttpPost("{sessionKey}/item/next")]
        public async Task<ActionResult<ItemDTO>> NextItem([FromHeader(Name = "PPAuthorization")] string authToken,
            string sessionKey)
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

            nextItem.Rounds.Add(new RoundDTO {Votes = new List<VoteDTO>()});
            session.Items[session.Items.FindIndex(item => item.Id == nextItem.Id)] = nextItem;

            await this.sessionRepository.UpdateAsync(EntityMapper.ToSessionCreateUpdateDto(session));

            return nextItem;
        }

        [HttpGet("{sessionKey}/item")]
        public async Task<ActionResult<ICollection<ItemDTO>>> GetAllItems(
            [FromHeader(Name = "PPAuthorization")] string authToken, string sessionKey)
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

        [HttpGet("{sessionKey}/item/current")]
        public async Task<ActionResult<ItemDTO>> GetCurrentItem([FromHeader(Name = "PPAuthorization")] string authToken,
            string sessionKey)
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

            var currentItem = SessionUtils.GetCurrentActiveItem(session.Items, session.Users.Count);

            if (!currentItem.HasValue)
            {
                return this.BadRequest();
            }

            return currentItem.ValueOrDefault();
        }

        [HttpPost("{sessionKey}/vote")]
        public Task<ActionResult> Vote([FromHeader(Name = "PPAuthorization")] string authToken, string sessionKey,
            [FromBody] VoteDTO vote)
        {
            throw new System.NotImplementedException();
        }

        [HttpPost("{sessionKey}/nitpicker")]
        public Task<ActionResult> ThrowNitpickerCard([FromHeader(Name = "PPAuthorization")] string authToken,
            string sessionKey)
        {
            throw new System.NotImplementedException();
        }

        [HttpPost("{sessionKey}/user/kick")]
        public Task<ActionResult> KickUser([FromHeader(Name = "PPAuthorization")] string authToken, string sessionKey,
            int userId)
        {
            throw new System.NotImplementedException();
        }
    }
}
