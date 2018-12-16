namespace PlanningPoker.WebApi.Tests.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using PlanningPoker.WebApi.Controllers;
    using Services;
    using Shared;
    using Xunit;

    public class SummaryControllerTest
    {
        [Fact]
        public async Task FindBySessionIdAsync_given_existing_id_returns_dto()
        {
            var dto = new SummaryDTO();
            var repository = new Mock<ISummaryRepository>();
            repository.Setup(s => s.FindBySessionIdAsync(42)).ReturnsAsync(dto);
            var controller = new SummaryController(repository.Object);
            var get = await controller.FindBySessionIdAsync(42);
            Assert.Equal(dto, get.Value);
        }

        [Fact]
        public async Task FindBySessionIdAsync_given_non_existing_id_returns_NotFound()
        {
            var repository = new Mock<ISummaryRepository>();
            var controller = new SummaryController(repository.Object);
            var get = await controller.FindBySessionIdAsync(42);
            Assert.IsType<NotFoundResult>(get.Result);
        }
    }
}
