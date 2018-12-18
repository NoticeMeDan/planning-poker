namespace PlanningPoker.WebApi.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using Shared;

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
        [HttpGet("{sessionId}")]
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
