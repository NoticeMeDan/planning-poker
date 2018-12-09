namespace PlanningPoker.WebApi.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using Shared;
    using Utils;

    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionRepository repository;

        public SessionController(ISessionRepository repo)
        {
            this.repository = repo;
        }

        // GET api/session/52A24B
        [HttpGet("{key}")]
        public async Task<ActionResult<SessionDTO>> GetByKey(string key)
        {
            var session = await this.repository.FindAsyncByKey(key);
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
                if (await this.repository.FindAsyncByKey(randomKey) == null)
                {
                    key = randomKey;
                }
            }

            session.SessionKey = key;
            var created = await this.repository.CreateAsync(session);
            return this.CreatedAtAction(nameof(this.GetByKey), new { created.Id }, created);
        }
    }
}
