namespace PlanningPoker.WebApi.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
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
            var output = new SessionDTO { Id = 42, SessionKey = "ABC1234" };
            var sessionRepo = new Mock<ISessionRepository>();
            sessionRepo.Setup(s => s.CreateAsync(input)).ReturnsAsync(output);
            var controller = new SessionController(sessionRepo.Object, null, null);
            var post = await controller.Create(input);
            var result = post.Result as CreatedAtActionResult;
            Assert.Equal("GetByKey", result.ActionName);
            Assert.Equal("ABC1234", result.RouteValues["SessionKey"]);
            Assert.Equal(output, result.Value);
        }

        [Fact]
        public async Task Join_given_nonexistant_sessionkey_returns_notfound()
        {
            var input = new UserCreateDTO { Nickname = "Marty McTestface" };
            var sessionRepo = new Mock<ISessionRepository>();
            var cache = new Mock<IMemoryCache>();
            var controller = new SessionController(sessionRepo.Object, null, cache.Object);
            var post = await controller.Join("ABC123", input);
            Assert.IsType<NotFoundResult>(post.Result);
        }

        [Fact]
        public async Task Join_given_host_where_session_already_has_hosts_returns_badrequest()
        {
            var sessionRepo = new Mock<ISessionRepository>();
            var cache = new Mock<IMemoryCache>();

            sessionRepo.Setup(s => s.FindAsyncByKey(It.IsAny<string>()))
                .ReturnsAsync(new SessionDTO
                    { SessionKey = "ABC1234", Users = new HashSet<UserDTO> { new UserDTO { IsHost = true } } });

            var controller = new SessionController(sessionRepo.Object, null, cache.Object);
            var input = new UserCreateDTO { Nickname = "Marty McTestface", IsHost = true };
            var post = await controller.Join("ABC123", input);
            Assert.IsType<BadRequestResult>(post.Result);
        }

        [Fact]
        public async Task Join_given_existant_sessionkey_and_user_joins_guest()
        {
            var sessionRepo = new Mock<ISessionRepository>();
            var userRepo = new Mock<IUserRepository>();
            var cache = new MemoryCache(new MemoryCacheOptions());

            sessionRepo.Setup(s => s.FindAsyncByKey(It.IsAny<string>()))
                .ReturnsAsync(new SessionDTO
                    { SessionKey = "ABC1234", Users = new HashSet<UserDTO> { new UserDTO { IsHost = true } } });

            userRepo.Setup(s => s.CreateAsync(It.IsAny<UserCreateDTO>()))
                .ReturnsAsync(new UserDTO { Id = 42, Nickname = "Marty McTestface" });

            var controller = new SessionController(sessionRepo.Object, userRepo.Object, cache);
            var input = new UserCreateDTO { Nickname = "Marty McTestface" };
            var post = await controller.Join("ABC1234", input);
            Assert.IsType<UserStateResponseDTO>(post.Value);
            Assert.IsType<string>(post.Value.Token);
            Assert.True(post.Value.Token != string.Empty);
        }

        [Fact]
        public async Task Join_given_existant_sessionkey_and_user_joins_scrummaster()
        {
            var sessionRepo = new Mock<ISessionRepository>();
            var userRepo = new Mock<IUserRepository>();
            var cache = new MemoryCache(new MemoryCacheOptions());

            sessionRepo.Setup(s => s.FindAsyncByKey(It.IsAny<string>()))
                .ReturnsAsync(new SessionDTO
                    { SessionKey = "ABC1234", Users = new HashSet<UserDTO> { new UserDTO { IsHost = false } } });

            userRepo.Setup(s => s.CreateAsync(It.IsAny<UserCreateDTO>()))
                .ReturnsAsync(new UserDTO { Id = 42, Nickname = "Marty McTestface", IsHost = true });

            var controller = new SessionController(sessionRepo.Object, userRepo.Object, cache);
            var input = new UserCreateDTO { Nickname = "Marty McTestface", IsHost = true };
            var post = await controller.Join("ABC1234", input);
            Assert.IsType<UserStateResponseDTO>(post.Value);
            Assert.IsType<string>(post.Value.Token);
            Assert.True(post.Value.Token != string.Empty);
        }
    }
}
