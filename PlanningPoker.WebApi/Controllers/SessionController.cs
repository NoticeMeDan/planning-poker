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
    using Shared;
    using Utils;

    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase, ISessionController
    {
        private readonly ISessionRepository sessionRepository;
        private readonly ISummaryRepository summaryRepository;
        private readonly UserStateManager userStateManager;

        public SessionController(ISessionRepository sessionRepo, IMemoryCache cache, ISummaryRepository summaryRepo)
        {
            this.sessionRepository = sessionRepo;
            this.summaryRepository = summaryRepo;
            this.userStateManager = new UserStateManager(cache);
        }

        // GET api/session/52A24B
        [HttpGet("{sessionKey}")]
        public async Task<ActionResult<SessionDTO>> GetByKey([FromHeader(Name = "PPAuthorization")] string authToken, string sessionKey)
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
            return created;
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

            var newUser = this.sessionRepository.AddUserToSession(user, session.Id);

            return new UserStateResponseDTO { Token = this.userStateManager.CreateState(newUser.Id, sessionKey) };
        }

        [Authorize]
        [HttpPost("{sessionKey}/item/round/next")]
        public async Task<ActionResult<RoundDTO>> NextRound([FromHeader(Name = "PPAuthorization")] string authToken, string sessionKey)
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

            var newRound = this.sessionRepository.AddRoundToSessionItem(currentItem.ValueOrDefault().Id);

            return newRound;
        }

        [HttpGet("{sessionKey}/item/round")]
        public async Task<ActionResult<RoundDTO>> GetCurrentRound([FromHeader(Name = "PPAuthorization")] string authToken, string sessionKey)
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
                await this.summaryRepository.BuildSummary(session);
                return this.BadRequest();
            }

            var newRound = this.sessionRepository.AddRoundToSessionItem(nextItem.Id);

            nextItem.Rounds.Add(newRound);
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
        public async Task<ActionResult<ItemDTO>> GetCurrentItem([FromHeader(Name = "PPAuthorization")] string authToken, string sessionKey)
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
        public async Task<ActionResult> Vote([FromHeader(Name = "PPAuthorization")] string authToken, string sessionKey, [FromBody] VoteCreateUpdateDTO vote)
        {
            if (!SecurityFilter.RequestIsValid(authToken, sessionKey, this.userStateManager))
            {
                return this.Unauthorized();
            }

            var session = await this.sessionRepository.FindByKeyAsync(sessionKey);

            if (session == null)
            {
                return this.StatusCode(404, "Session not found");
            }

            var currentItem = SessionUtils.GetCurrentActiveItem(session.Items, session.Users.Count);

            if (!currentItem.HasValue)
            {
                return this.StatusCode(404, "Active Item not found");
            }

            var currentRound = SessionUtils.GetCurrentActiveRound(currentItem.ValueOrDefault().Rounds.ToList(), session.Users.Count);

            if (!currentRound.HasValue)
            {
                return this.StatusCode(404, "Active Round not found");
            }

            var userState = this.userStateManager.GetState(authToken.Replace("Bearer ", string.Empty)).ValueOrDefault();

            this.sessionRepository.AddVoteToRound(
                    new VoteCreateUpdateDTO { Estimate = vote.Estimate, UserId = userState.Id },
                    currentRound.ValueOrDefault().Id);

            return this.Ok();
        }

        [HttpGet("{sessionKey}/whoami")]
        public ActionResult<UserState> WhoAmI([FromHeader(Name = "PPAuthorization")] string authToken, string sessionKey)
        {
            if (!SecurityFilter.RequestIsValid(authToken, sessionKey, this.userStateManager))
            {
                return this.Unauthorized();
            }

            var state = this.userStateManager.GetState(authToken.Replace("Bearer ", string.Empty));

            return state.ValueOrDefault();
        }

        [HttpPost("{sessionKey}/user/kick")]
        public Task<ActionResult> KickUser([FromHeader(Name = "PPAuthorization")] string authToken, string sessionKey, int userId)
        {
            throw new System.NotImplementedException();
        }

        // [HttpPost("{sessionKey}/nitpicker")]
        public Task<ActionResult> ThrowNitpickerCard([FromHeader(Name = "PPAuthorization")] string authToken, string sessionKey)
        {
            throw new System.NotImplementedException();
        }
    }
}
