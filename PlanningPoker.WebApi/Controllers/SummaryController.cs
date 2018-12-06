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
    public class SummaryController : ControllerBase
    {
        private readonly ISummaryRepository repository;

        public SummaryController(ISummaryRepository repo)
        {
            this.repository = repo;
        }

        // GET api/summaries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SummaryDTO>>> Get()
        {
            return await this.repository.Read().ToListAsync();
        }

        // GET api/summaries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SummaryDTO>> Get(int id)
        {
            var summary = await this.repository.FindAsync(id);

            if (summary == null)
            {
                return this.NotFound();
            }

            return summary;
        }

        // POST api/summary
        [HttpPost]
        public async Task<ActionResult<SummaryDTO>> Post([FromBody] SummaryCreateUpdateDTO summary)
        {
            var created = await this.repository.CreateAsync(summary);
             return this.CreatedAtAction(nameof(this.Get), new { created.Id }, created);
        }

        // PUT api/summaries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] SummaryCreateUpdateDTO summary)
        {
            var updated = await this.repository.UpdateAsync(summary);

            if (updated)
            {
                return this.NoContent();
            }

            return this.NotFound();
        }

        // DELETE: api/summaries/5
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
