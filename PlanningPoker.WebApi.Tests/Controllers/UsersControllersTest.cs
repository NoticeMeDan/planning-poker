namespace PlanningPoker.WebApi.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using MockQueryable.Moq;
    using Moq;
    using PlanningPoker.Services;
    using PlanningPoker.Shared;
    using PlanningPoker.WebApi.Controllers;
    using Xunit;

    public class UsersControllersTest
    {
        [Fact]
        public async Task Get_returns_dtos()
        {
            var dto = new UserDTO();
            var all = new[] { dto }.AsQueryable().BuildMock();
            var repository = new Mock<IUserRepository>();
            repository.Setup(s => s.Read()).Returns(all.Object);

            var controller = new UsersController(repository.Object);

            var result = await controller.Get();

            Assert.Equal(dto, result.Value.Single());
        }

        [Fact]
        public async Task Get_given_existing_id_returns_dto()
        {
            var dto = new UserDTO();
            var repository = new Mock<IUserRepository>();
            repository.Setup(s => s.FindAsync(42)).ReturnsAsync(dto);

            var controller = new UsersController(repository.Object);

            var get = await controller.Get(42);

            Assert.Equal(dto, get.Value);
        }

        [Fact]
        public async Task Get_given_non_existing_id_returns_NotFound()
        {
            var repository = new Mock<IUserRepository>();

            var controller = new UsersController(repository.Object);

            var get = await controller.Get(42);

            Assert.IsType<NotFoundResult>(get.Result);
        }

        [Fact]
        public async Task Post_given_dto_creates_user()
        {
            var output = new UserDTO();
            var repository = new Mock<IUserRepository>();
            repository.Setup(s => s.CreateAsync(It.IsAny<UserCreateUpdateDTO>())).ReturnsAsync(output);

            var controller = new UsersController(repository.Object);

            var input = new UserCreateUpdateDTO();

            await controller.Post(input);

            repository.Verify(s => s.CreateAsync(input));
        }

        [Fact]
        public async Task Post_given_dto_returns_CreatedAtActionResult()
        {
            var input = new UserCreateUpdateDTO();
            var output = new UserDTO { Id = 42 };
            var repository = new Mock<IUserRepository>();
            repository.Setup(s => s.CreateAsync(input)).ReturnsAsync(output);

            var controller = new UsersController(repository.Object);

            var post = await controller.Post(input);
            var result = post.Result as CreatedAtActionResult;

            Assert.Equal("Get", result.ActionName);
            Assert.Equal(42, result.RouteValues["id"]);
            Assert.Equal(output, result.Value);
        }


        [Fact]
        public async Task Put_given_dto_updates_user()
        {
            var repository = new Mock<IUserRepository>();

            var controller = new UsersController(repository.Object);

            var dto = new UserCreateUpdateDTO();

            await controller.Put(42, dto);

            repository.Verify(s => s.UpdateAsync(dto));
        }

        [Fact]
        public async Task Put_returns_NoContent()
        {
            var dto = new UserCreateUpdateDTO();
            var repository = new Mock<IUserRepository>();
            repository.Setup(s => s.UpdateAsync(dto)).ReturnsAsync(true);
            var controller = new UsersController(repository.Object);

            var put = await controller.Put(42, dto);

            Assert.IsType<NoContentResult>(put);
        }

        [Fact]
        public async Task Put_given_repository_returns_false_returns_NotFound()
        {
            var repository = new Mock<IUserRepository>();

            var controller = new UsersController(repository.Object);

            var dto = new UserCreateUpdateDTO();

            var put = await controller.Put(42, dto);

            Assert.IsType<NotFoundResult>(put);
        }
    }
}
