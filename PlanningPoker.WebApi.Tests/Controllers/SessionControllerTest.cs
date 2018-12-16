namespace PlanningPoker.WebApi.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Moq;
    using PlanningPoker.WebApi.Controllers;
    using PlanningPoker.WebApi.Security;
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
            sessionRepo.Setup(s => s.FindByKeyAsync("ABC123")).ReturnsAsync(dto);
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
            var result = post.Value;
            Assert.Equal("ABC1234", result.SessionKey);
            Assert.Equal(42, result.Id);
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
        public async Task Join_given_host_where_session_already_has_host_returns_badrequest()
        {
            var sessionRepo = new Mock<ISessionRepository>();
            var cache = new Mock<IMemoryCache>();

            sessionRepo.Setup(s => s.FindByKeyAsync(It.IsAny<string>()))
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

            sessionRepo.Setup(s => s.FindByKeyAsync(It.IsAny<string>()))
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

            sessionRepo.Setup(s => s.FindByKeyAsync(It.IsAny<string>()))
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

        [Fact]
        public async Task NextItem_given_invalid_token_returns_forbidden()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());

            var controller = new SessionController(null, null, cache);

            var result = await controller.NextItem("IDoNotExist", "ABC1234");

            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        [Fact]
        public async Task NextItem_given_nonexistant_sessionkey_returns_notfound()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var sessionRepo = new Mock<ISessionRepository>();

            var token = CreateUserState(cache, 42, "ABC1234");
            sessionRepo.Setup(s => s.FindByKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(default(SessionDTO));

            var controller = new SessionController(sessionRepo.Object, null, cache);

            var result = await controller.NextItem(token, "ABC1234");

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task NextItem_given_completed_session_returns_badrequest()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var sessionRepo = new Mock<ISessionRepository>();

            var token = CreateUserState(cache, 42, "ABC1234");

            var mockSession = new SessionDTO
            {
                Items = new List<ItemDTO>
                {
                    new ItemDTO
                    {
                        Rounds = new List<RoundDTO>
                        {
                            new RoundDTO(),
                            new RoundDTO()
                        }
                    }
                }
            };
            sessionRepo.Setup(s => s.FindByKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(mockSession);

            var controller = new SessionController(sessionRepo.Object, null, cache);

            var result = await controller.NextItem(token, "ABC1234");

            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task NextItem_given_active_session_returns_nextItem()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var sessionRepo = new Mock<ISessionRepository>();

            var token = CreateUserState(cache, 42, "ABC1234");

            var mockSession = new SessionDTO
            {
                Id = 42,
                Items = new List<ItemDTO>
                {
                    new ItemDTO
                    {
                        Rounds = new List<RoundDTO>
                        {
                            new RoundDTO(),
                            new RoundDTO()
                        }
                    },
                    new ItemDTO { Rounds = new List<RoundDTO>() }
                },
                SessionKey = "ABC1234",
                Users = new List<UserDTO>()
            };

            sessionRepo.Setup(s => s.FindByKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(mockSession);

            sessionRepo.Setup(s => s.UpdateAsync(It.IsAny<SessionCreateUpdateDTO>()))
                .ReturnsAsync(true);

            var controller = new SessionController(sessionRepo.Object, null, cache);

            var result = await controller.NextItem(token, "ABC1234");

            Assert.IsType<ItemDTO>(result.Value);
            Assert.Equal(1, result.Value.Rounds.Count);
        }

        [Fact]
        public async Task GetCurrentItem_given_invalid_token_returns_forbidden()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());

            var controller = new SessionController(null, null, cache);

            var result = await controller.GetCurrentItem("IDoNotExist", "ABC1234");

            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        [Fact]
        public async Task GetCurrentItem_given_nonexistant_sessionkey_returns_notfound()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var sessionRepo = new Mock<ISessionRepository>();

            var token = CreateUserState(cache, 42, "ABC1234");
            sessionRepo.Setup(s => s.FindByKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(default(SessionDTO));

            var controller = new SessionController(sessionRepo.Object, null, cache);

            var result = await controller.GetCurrentItem(token, "ABC1234");

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetCurrentItem_given_not_started_session_returns_badrequest()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var sessionRepo = new Mock<ISessionRepository>();

            var token = CreateUserState(cache, 42, "ABC1234");

            var mockSession = new SessionDTO
            {
                Id = 42,
                Items = new List<ItemDTO>
                {
                    new ItemDTO { Rounds = new List<RoundDTO>() }
                },
                SessionKey = "ABC1234",
                Users = new List<UserDTO>()
            };

            sessionRepo.Setup(s => s.FindByKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(mockSession);

            var controller = new SessionController(sessionRepo.Object, null, cache);

            var result = await controller.GetCurrentItem(token, "ABC1234");

            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task GetCurrentItem_given_finished_session_returns_badRequest()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var sessionRepo = new Mock<ISessionRepository>();

            var token = CreateUserState(cache, 42, "ABC1234");

            var mockSession = new SessionDTO
            {
                Id = 42,
                Items = new List<ItemDTO>
                {
                    new ItemDTO
                    {
                        Id = 42,
                        Rounds = new List<RoundDTO>
                        {
                            new RoundDTO
                            {
                                Id = 1,
                                Votes = new List<VoteDTO>
                                {
                                    new VoteDTO { Estimate = 13 },
                                    new VoteDTO { Estimate = 13 }
                                }
                            }
                        }
                    }
                },
                SessionKey = "ABC1234",
                Users = new List<UserDTO> { new UserDTO(), new UserDTO() }
            };

            sessionRepo.Setup(s => s.FindByKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(mockSession);

            var controller = new SessionController(sessionRepo.Object, null, cache);

            var result = await controller.GetCurrentItem(token, "ABC1234");

            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task GetCurrentItem_given_running_session_returns_current_item()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var sessionRepo = new Mock<ISessionRepository>();

            var token = CreateUserState(cache, 42, "ABC1234");

            var mockSession = new SessionDTO
            {
                Id = 42,
                Items = new List<ItemDTO>
                {
                    new ItemDTO
                    {
                        Id = 1,
                        Rounds = new List<RoundDTO>
                        {
                            new RoundDTO
                            {
                                Id = 1,
                                Votes = new List<VoteDTO>
                                {
                                    new VoteDTO { Estimate = 13 },
                                    new VoteDTO { Estimate = 13 }
                                }
                            }
                        }
                    },
                    new ItemDTO
                    {
                        Id = 2,
                        Rounds = new List<RoundDTO>
                        {
                            new RoundDTO
                            {
                                Id = 1,
                                Votes = new List<VoteDTO>
                                {
                                    new VoteDTO { Estimate = 13 },
                                    new VoteDTO { Estimate = 5 }
                                }
                            }
                        }
                    }
                },
                SessionKey = "ABC1234",
                Users = new List<UserDTO> { new UserDTO(), new UserDTO() }
            };

            sessionRepo.Setup(s => s.FindByKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(mockSession);

            var controller = new SessionController(sessionRepo.Object, null, cache);

            var result = await controller.GetCurrentItem(token, "ABC1234");

            Assert.IsType<ItemDTO>(result.Value);
            Assert.Equal(2, result.Value.Id);
        }

        [Fact]
        public async Task GetAllItems_given_invalid_token_returns_unauthorized()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());

            var controller = new SessionController(null, null, cache);

            var result = await controller.GetAllItems("IDontExist", "ABC1234");

            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        [Fact]
        public async Task GetAllItems_given_nonexisting_session_returns_notfound()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var sessionRepo = new Mock<ISessionRepository>();

            var token = CreateUserState(cache, 42, "ABC1234");
            sessionRepo.Setup(s => s.FindByKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(default(SessionDTO));

            var controller = new SessionController(sessionRepo.Object, null, cache);

            var result = await controller.GetAllItems(token, "ABC1234");

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAllItems_given_existing_session_returns_items()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var sessionRepo = new Mock<ISessionRepository>();

            var token = CreateUserState(cache, 42, "ABC1234");

            var mockSession = new SessionDTO
            {
                Id = 42,
                Items = new List<ItemDTO>
                {
                    new ItemDTO
                    {
                        Id = 1,
                        Rounds = new List<RoundDTO>()
                    },
                    new ItemDTO
                    {
                        Id = 2,
                        Rounds = new List<RoundDTO>()
                    }
                },
                SessionKey = "ABC1234",
                Users = new List<UserDTO> { new UserDTO(), new UserDTO() }
            };

            sessionRepo.Setup(s => s.FindByKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(mockSession);

            var controller = new SessionController(sessionRepo.Object, null, cache);

            var result = await controller.GetAllItems(token, "ABC1234");

            Assert.IsType<List<ItemDTO>>(result.Value);
            Assert.Equal(2, result.Value.Count);
            Assert.Equal(1, result.Value.ToList()[0].Id);
            Assert.Equal(2, result.Value.ToList()[1].Id);
        }

        [Fact]
        public async Task NextRound_given_invalid_token_returns_unauthorized()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());

            var controller = new SessionController(null, null, cache);

            var result = await controller.NextRound("IDontExist", "ABC1234");

            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        [Fact]
        public async Task NextRound_given_nonexisting_session_returns_notfound()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var sessionRepo = new Mock<ISessionRepository>();

            var token = CreateUserState(cache, 42, "ABC1234");
            sessionRepo.Setup(s => s.FindByKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(default(SessionDTO));

            var controller = new SessionController(sessionRepo.Object, null, cache);

            var result = await controller.NextRound(token, "ABC1234");

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task NextRound_given_round_with_consensus_returns_badrequest()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var sessionRepo = new Mock<ISessionRepository>();

            var token = CreateUserState(cache, 42, "ABC1234");

            var mockSession = new SessionDTO
            {
                Id = 42,
                Items = new List<ItemDTO>
                {
                    new ItemDTO
                    {
                        Id = 1,
                        Rounds = new List<RoundDTO>
                        {
                            new RoundDTO
                            {
                                Votes = new List<VoteDTO> { new VoteDTO { Estimate = 5 }, new VoteDTO { Estimate = 5 } }
                            }
                        }
                    }
                },
                SessionKey = "ABC1234",
                Users = new List<UserDTO> { new UserDTO(), new UserDTO() }
            };

            sessionRepo.Setup(s => s.FindByKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(mockSession);

            var controller = new SessionController(sessionRepo.Object, null, cache);

            var result = await controller.NextRound(token, "ABC1234");

            Assert.IsType<BadRequestResult>(result.Result);
        }

        public async Task NextRound_given_round_without_consensus_return_new_round()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var sessionRepo = new Mock<ISessionRepository>();

            var token = CreateUserState(cache, 42, "ABC1234");

            var mockSession = new SessionDTO
            {
                Id = 42,
                Items = new List<ItemDTO>
                {
                    new ItemDTO
                    {
                        Id = 1,
                        Rounds = new List<RoundDTO>
                        {
                            new RoundDTO
                            {
                                Votes = new List<VoteDTO> { new VoteDTO { Estimate = 5 }, new VoteDTO { Estimate = 13 } }
                            }
                        }
                    }
                },
                SessionKey = "ABC1234",
                Users = new List<UserDTO> { new UserDTO(), new UserDTO() }
            };

            sessionRepo.Setup(s => s.FindByKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(mockSession);

            var controller = new SessionController(sessionRepo.Object, null, cache);

            var result = await controller.NextRound(token, "ABC1234");

            Assert.Equal(0, result.Value.Votes.Count);
        }

        private static string CreateUserState(IMemoryCache cache, int userId, string sessionKey)
        {
            var sm = new UserStateManager(cache);
            return sm.CreateState(userId, sessionKey);
        }
    }
}
