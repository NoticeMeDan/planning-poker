namespace PlanningPoker.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PlanningPoker.Services;
    using PlanningPoker.Shared;

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository repository;

        public UsersController(IUserRepository repo)
        {
            this.repository = repo;
        }

        // GET api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> Get()
        {
            return await this.repository.Read().ToListAsync();
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> Get(int id)
        {
            var user = await this.repository.FindAsync(id);

            if (user == null)
            {
                return this.NotFound();
            }

            return user;
        }

        // POST api/users
        [HttpPost]
        public async Task<ActionResult<UserDTO>> Post([FromBody] UserCreateUpdateDTO user)
        {
            var created = await this.repository.CreateAsync(user);

            return this.CreatedAtAction(nameof(this.Get), new { created.Id }, created);
        }

        // PUT api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserCreateUpdateDTO user)
        {
            var updated = await this.repository.UpdateAsync(user);

            if (updated)
            {
                return this.NoContent();
            }

            return this.NotFound();
        }

        // DELETE: api/users/5
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
