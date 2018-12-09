namespace PlanningPoker.WebApi.Tests.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using PlanningPoker.WebApi.Controllers;
    using Services;
    using Shared;
    using Xunit;

    public class SessionControllerTest
    {
        [Fact]
        public async Task GetByKey_given_existing_key_returns_dto()
        {
            var dto = new SessionDTO();
            var sessionRepo = new Mock<ISessionRepository>();
            sessionRepo.Setup(s => s.FindAsyncByKey("ABC123")).ReturnsAsync(dto);
            var controller = new SessionController(sessionRepo.Object, null, null);
            var get = await controller.GetByKey("ABC123");
            Assert.Equal(dto, get.Value);
        }

        [Fact]
        public async Task GetByKey_given_non_existing_key_returns_NotFound()
        {
            var sessionRepo = new Mock<ISessionRepository>();
            var controller = new SessionController(sessionRepo.Object, null, null);
            var get = await controller.GetByKey("ABC123");
            Assert.IsType<NotFoundResult>(get.Result);
        }

        [Fact]
        public async Task Create_given_dto_creates_session()
        {
            var output = new SessionDTO();
            var sessionRepo = new Mock<ISessionRepository>();
            sessionRepo.Setup(s => s.CreateAsync(It.IsAny<SessionCreateUpdateDTO>())).ReturnsAsync(output);
            var controller = new SessionController(sessionRepo.Object, null, null);
            var input = new SessionCreateUpdateDTO();
            await controller.Create(input);
            sessionRepo.Verify(s => s.CreateAsync(input));
        }

        [Fact]
        public async Task Create_given_dto_returns_CreatedAtActionResult()
        {
            var input = new SessionCreateUpdateDTO();
            var output = new SessionDTO { Id = 42 };
            var sessionRepo = new Mock<ISessionRepository>();
            sessionRepo.Setup(s => s.CreateAsync(input)).ReturnsAsync(output);
            var controller = new SessionController(sessionRepo.Object, null, null);
            var post = await controller.Create(input);
            var result = post.Result as CreatedAtActionResult;
            Assert.Equal("GetByKey", result.ActionName);
            Assert.Equal(42, result.RouteValues["id"]);
            Assert.Equal(output, result.Value);
        }

        [Fact]
        public async Task Join_given_nonexistant_sessionkey_returns_notfound()
        {

        }

        [Fact]
        public async Task Join_given_host_where_session_already_has_hosts_returns_badrequest()
        {

        }

        [Fact]
        public async Task Join_given_existant_sessionkey_and_user_joins_user()
        {

        }
    }
}
