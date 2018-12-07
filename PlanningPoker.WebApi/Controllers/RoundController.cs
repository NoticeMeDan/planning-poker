using PlanningPoker.WebApi.Hubs;

namespace PlanningPoker.WebApi.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoundController : ControllerBase
    {
        private VotesHub hub = new VotesHub();

        // GET api/rounds
        [HttpGet]

        // [AllowAnonymous]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/rounds/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "round";
        }

        // POST api/rounds
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/rounds/5
        [HttpPut("{id}")]
        public async void PutAsync(int id, [FromBody] string value)
        {
            //Put a Vote into a Round.

            //If succesful, send a message to all clients that a vote was added.
            hub.SendVote(id, value);
        }

        // DELETE api/rounds/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
