namespace PlanningPoker.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PlanningPoker.Services;
    using PlanningPoker.Shared;

    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionRepository repository;

        public SessionController(ISessionRepository repo)
        {
            this.repository = repo;
        }

        // GET api/session
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionDTO>>> Get()
        {
            return await this.repository.Read().ToListAsync();
        }

        // GET api/session/52A24B
        [HttpGet("{key}")]
        public async Task<ActionResult<SessionDTO>> Get(string key)
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
        public async Task<ActionResult<SessionDTO>> Post([FromBody] SessionCreateUpdateDTO session)
        {
            var created = await this.repository.CreateAsync(session);
             return this.CreatedAtAction(nameof(this.Get), new { created.Id }, created);
        }

        // PUT api/session/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] SessionCreateUpdateDTO session)
        {
            var updated = await this.repository.UpdateAsync(session);
             if (updated)
            {
                return this.NoContent();
            }

            return this.NotFound();
        }

        // DELETE: api/session/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await this.repository.DeleteAsync(id);

            if (result)
            {
                return this.NoContent();
            }

            return this.NotFound();
        }
    }
}
