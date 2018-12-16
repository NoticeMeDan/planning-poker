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

        // GET api/summary/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SummaryDTO>> FindBySessionIdAsync(int sessionId)
        {
            var summary = await this.repository.FindBySessionIdAsync(sessionId);

            if (summary == null)
            {
                return this.NotFound();
            }

            return summary;
        }
    }
}
