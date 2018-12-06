namespace PlanningPoker.WebApi.Tests.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using MockQueryable.Moq;
    using Moq;
    using PlanningPoker.Services;
    using PlanningPoker.Shared;
    using PlanningPoker.WebApi.Controllers;
    using Xunit;

    public class SummaryControllerTest
    {
        [Fact]
        public async Task Get_returns_dtos()
        {
            var dto = new SummaryDTO();
            var all = new[] { dto }.AsQueryable().BuildMock();
            var repository = new Mock<ISummaryRepository>();
            repository.Setup(s => s.Read()).Returns(all.Object);
             var controller = new SummaryController(repository.Object);
             var result = await controller.Get();
             Assert.Equal(dto, result.Value.Single());
        }

        [Fact]
        public async Task Get_given_existing_id_returns_dto()
        {
            var dto = new SummaryDTO();
            var repository = new Mock<ISummaryRepository>();
            repository.Setup(s => s.FindAsync(42)).ReturnsAsync(dto);
             var controller = new SummaryController(repository.Object);
             var get = await controller.Get(42);
             Assert.Equal(dto, get.Value);
        }

        [Fact]
        public async Task Get_given_non_existing_id_returns_NotFound()
        {
            var repository = new Mock<ISummaryRepository>();
             var controller = new SummaryController(repository.Object);
             var get = await controller.Get(42);
             Assert.IsType<NotFoundResult>(get.Result);
        }

        [Fact]
        public async Task Post_given_dto_creates_summary()
        {
            var output = new SummaryDTO();
            var repository = new Mock<ISummaryRepository>();
            repository.Setup(s => s.CreateAsync(It.IsAny<SummaryCreateUpdateDTO>())).ReturnsAsync(output);
             var controller = new SummaryController(repository.Object);
             var input = new SummaryCreateUpdateDTO();
             await controller.Post(input);
             repository.Verify(s => s.CreateAsync(input));
        }

        [Fact]
        public async Task Post_given_dto_returns_CreatedAtActionResult()
        {
            var input = new SummaryCreateUpdateDTO();
            var output = new SummaryDTO { Id = 42 };
            var repository = new Mock<ISummaryRepository>();
            repository.Setup(s => s.CreateAsync(input)).ReturnsAsync(output);
             var controller = new SummaryController(repository.Object);
             var post = await controller.Post(input);
            var result = post.Result as CreatedAtActionResult;
             Assert.Equal("Get", result.ActionName);
            Assert.Equal(42, result.RouteValues["id"]);
            Assert.Equal(output, result.Value);
        }

        [Fact]
        public async Task Put_given_dto_updates_summary()
        {
            var repository = new Mock<ISummaryRepository>();
             var controller = new SummaryController(repository.Object);
             var dto = new SummaryCreateUpdateDTO();
             await controller.Put(42, dto);
             repository.Verify(s => s.UpdateAsync(dto));
        }

        [Fact]
        public async Task Put_returns_NoContent()
        {
            var dto = new SummaryCreateUpdateDTO();
            var repository = new Mock<ISummaryRepository>();
            repository.Setup(s => s.UpdateAsync(dto)).ReturnsAsync(true);
            var controller = new SummaryController(repository.Object);
             var put = await controller.Put(42, dto);
             Assert.IsType<NoContentResult>(put);
        }

        [Fact]
        public async Task Put_given_repository_returns_false_returns_NotFound()
        {
            var repository = new Mock<ISummaryRepository>();
             var controller = new SummaryController(repository.Object);
             var dto = new SummaryCreateUpdateDTO();
             var put = await controller.Put(42, dto);
             Assert.IsType<NotFoundResult>(put);
        }

        [Fact]
        public async Task Delete_given_id_deletes_summary()
        {
            var repository = new Mock<ISummaryRepository>();
             var controller = new SummaryController(repository.Object);
             await controller.Delete(42);
             repository.Verify(s => s.DeleteAsync(42));
        }

        [Fact]
        public async Task Delete_returns_NoContent()
        {
            var repository = new Mock<ISummaryRepository>();
            repository.Setup(s => s.DeleteAsync(42)).ReturnsAsync(true);
            var controller = new SummaryController(repository.Object);
             var delete = await controller.Delete(42);
             Assert.IsType<NoContentResult>(delete);
        }

        [Fact]
        public async Task Delete_given_repository_returns_false_returns_NotFound()
        {
            var repository = new Mock<ISummaryRepository>();
             var controller = new SummaryController(repository.Object);
             var delete = await controller.Delete(42);
             Assert.IsType<NotFoundResult>(delete);
        }
    }
}
