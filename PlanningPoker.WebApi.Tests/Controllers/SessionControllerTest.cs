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
        public async Task Get_given_existing_id_returns_dto()
        {
            var dto = new SessionDTO();
            var repository = new Mock<ISessionRepository>();
            repository.Setup(s => s.FindAsyncByKey("ABC123")).ReturnsAsync(dto);
            var controller = new SessionController(repository.Object);
            var get = await controller.GetByKey("ABC123");
            Assert.Equal(dto, get.Value);
        }

        [Fact]
        public async Task Get_given_non_existing_id_returns_NotFound()
        {
            var repository = new Mock<ISessionRepository>();
            var controller = new SessionController(repository.Object);
            var get = await controller.GetByKey("ABC123");
            Assert.IsType<NotFoundResult>(get.Result);
        }

        [Fact]
        public async Task Post_given_dto_creates_session()
        {
            var output = new SessionDTO();
            var repository = new Mock<ISessionRepository>();
            repository.Setup(s => s.CreateAsync(It.IsAny<SessionCreateUpdateDTO>())).ReturnsAsync(output);
            var controller = new SessionController(repository.Object);
            var input = new SessionCreateUpdateDTO();
            await controller.Create(input);
            repository.Verify(s => s.CreateAsync(input));
        }

        [Fact]
        public async Task Post_given_dto_returns_CreatedAtActionResult()
        {
            var input = new SessionCreateUpdateDTO();
            var output = new SessionDTO { Id = 42 };
            var repository = new Mock<ISessionRepository>();
            repository.Setup(s => s.CreateAsync(input)).ReturnsAsync(output);
            var controller = new SessionController(repository.Object);
            var post = await controller.Create(input);
            var result = post.Result as CreatedAtActionResult;
            Assert.Equal("GetByKey", result.ActionName);
            Assert.Equal(42, result.RouteValues["id"]);
            Assert.Equal(output, result.Value);
        }
    }
}
