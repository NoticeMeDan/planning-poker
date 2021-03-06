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
            var cache = new MemoryCache(new MemoryCacheOptions());

            sessionRepo.Setup(s => s.FindByKeyAsync("ABC1234")).ReturnsAsync(dto);
            var controller = new SessionController(sessionRepo.Object, cache, null);

            var token = CreateUserState(cache, 42, "ABC1234");

            var get = await controller.GetByKey(token, "ABC1234");
            Assert.Equal(dto, get.Value);
        }

        [Fact]
        public async Task GetByKey_given_non_existing_key_returns_NotFound()
        {
            var sessionRepo = new Mock<ISessionRepository>();
            var cache = new MemoryCache(new MemoryCacheOptions());
            var controller = new SessionController(sessionRepo.Object, cache, null);

            var token = CreateUserState(cache, 42, "ABC1234");

            var get = await controller.GetByKey(token, "ABC1234");
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
            var controller = new SessionController(sessionRepo.Object, cache.Object, null);
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

            var controller = new SessionController(sessionRepo.Object, cache.Object, null);
            var input = new UserCreateDTO { Nickname = "Marty McTestface", IsHost = true };
            var post = await controller.Join("ABC123", input);
            Assert.IsType<BadRequestResult>(post.Result);
        }

        [Fact]
        public async Task Join_given_existant_sessionkey_and_user_joins_guest()
        {
            var sessionRepo = new Mock<ISessionRepository>();
            var cache = new MemoryCache(new MemoryCacheOptions());

            sessionRepo.Setup(s => s.FindByKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(new SessionDTO
                    { SessionKey = "ABC1234", Users = new HashSet<UserDTO> { new UserDTO { IsHost = true } } });

            sessionRepo.Setup(s => s.AddUserToSession(It.IsAny<UserCreateDTO>(), It.IsAny<int>()))
                .Returns(new UserDTO { Id = 42 });

            var controller = new SessionController(sessionRepo.Object, cache, null);
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
            var cache = new MemoryCache(new MemoryCacheOptions());

            sessionRepo.Setup(s => s.FindByKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(new SessionDTO
                    { SessionKey = "ABC1234", Users = new HashSet<UserDTO> { new UserDTO { IsHost = false } } });

            sessionRepo.Setup(s => s.AddUserToSession(It.IsAny<UserCreateDTO>(), It.IsAny<int>()))
                .Returns(new UserDTO { Id = 42 });

            var controller = new SessionController(sessionRepo.Object, cache, null);
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

            var controller = new SessionController(null, cache, null);

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

            var controller = new SessionController(sessionRepo.Object, cache, null);

            var result = await controller.NextItem(token, "ABC1234");

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task NextItem_given_completed_session_returns_badrequest()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var sessionRepo = new Mock<ISessionRepository>();
            var summaryRepo = new Mock<ISummaryRepository>();

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

            var mockSummary = new SummaryDTO
            {
                ItemEstimates = new List<ItemEstimateDTO>
                {
                    new ItemEstimateDTO(),
                    new ItemEstimateDTO()
                }
            };
            sessionRepo.Setup(s => s.FindByKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(mockSession);
            summaryRepo.Setup(s => s.BuildSummary(mockSession)).
                ReturnsAsync(mockSummary);

            var controller = new SessionController(sessionRepo.Object, cache, summaryRepo.Object);

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

            var controller = new SessionController(sessionRepo.Object, cache, null);

            var result = await controller.NextItem(token, "ABC1234");

            Assert.IsType<ItemDTO>(result.Value);
            Assert.Equal(1, result.Value.Rounds.Count);
        }

        [Fact]
        public async Task GetCurrentItem_given_invalid_token_returns_forbidden()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());

            var controller = new SessionController(null, cache, null);

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

            var controller = new SessionController(sessionRepo.Object, cache, null);

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

            var controller = new SessionController(sessionRepo.Object, cache, null);

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

            var controller = new SessionController(sessionRepo.Object, cache, null);

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

            var controller = new SessionController(sessionRepo.Object, cache, null);

            var result = await controller.GetCurrentItem(token, "ABC1234");

            Assert.IsType<ItemDTO>(result.Value);
            Assert.Equal(2, result.Value.Id);
        }

        [Fact]
        public async Task GetAllItems_given_invalid_token_returns_unauthorized()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());

            var controller = new SessionController(null, cache, null);

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

            var controller = new SessionController(sessionRepo.Object, cache, null);

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

            var controller = new SessionController(sessionRepo.Object, cache, null);

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

            var controller = new SessionController(null, cache, null);

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

            var controller = new SessionController(sessionRepo.Object, cache, null);

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

            var controller = new SessionController(sessionRepo.Object, cache, null);

            var result = await controller.NextRound(token, "ABC1234");

            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
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

            sessionRepo.Setup(s => s.AddRoundToSessionItem(It.IsAny<int>()))
                .Returns(new RoundDTO { Votes = new List<VoteDTO>() });

            var controller = new SessionController(sessionRepo.Object, cache, null);

            var result = await controller.NextRound(token, "ABC1234");

            Assert.Equal(0, result.Value.Votes.Count);
        }

        [Fact]
        public async Task GetCurrentRound_given_invalid_token_returns_unauthorized()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());

            var controller = new SessionController(null, cache, null);

            var result = await controller.GetCurrentRound("IDontExist", "ABC1234");

            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        [Fact]
        public async Task GetCurrentRound_given_nonexisting_session_returns_notfound()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var sessionRepo = new Mock<ISessionRepository>();

            var token = CreateUserState(cache, 42, "ABC1234");
            sessionRepo.Setup(s => s.FindByKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(default(SessionDTO));

            var controller = new SessionController(sessionRepo.Object, cache, null);

            var result = await controller.GetCurrentRound(token, "ABC1234");

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetCurrentRound_given_round_withconsensus_return_badrequest()
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
                                Votes = new List<VoteDTO> { new VoteDTO { Estimate = 13 }, new VoteDTO { Estimate = 13 } }
                            }
                        }
                    }
                },
                SessionKey = "ABC1234",
                Users = new List<UserDTO> { new UserDTO(), new UserDTO() }
            };

            sessionRepo.Setup(s => s.FindByKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(mockSession);

            var controller = new SessionController(sessionRepo.Object, cache, null);

            var result = await controller.GetCurrentRound(token, "ABC1234");

            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task GetCurrentRound_given_round_without_consensus_return_round()
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

            var controller = new SessionController(sessionRepo.Object, cache, null);

            var result = await controller.GetCurrentRound(token, "ABC1234");

            Assert.Equal(2, result.Value.Votes.Count);
        }

        [Fact]
        public async Task Vote_given_invalid_token_returns_unauthorized()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());

            var controller = new SessionController(null, cache, null);

            var result = await controller.Vote("IDontExist", "ABC1234", new VoteCreateUpdateDTO());

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Vote_given_nonExistant_session_return_notFound()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var sessionRepo = new Mock<ISessionRepository>();

            var token = CreateUserState(cache, 42, "ABC1234");
            sessionRepo.Setup(s => s.FindByKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(default(SessionDTO));

            var controller = new SessionController(sessionRepo.Object, cache, null);

            var result = await controller.Vote(token, "ABC1234", new VoteCreateUpdateDTO()) as ObjectResult;

            Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Session not found", result.Value);
        }

        [Fact]
        public async Task Vote_given_nonActive_item_returns_notFound()
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

            var controller = new SessionController(sessionRepo.Object, cache, null);

            var result = await controller.Vote(token, "ABC1234", new VoteCreateUpdateDTO { Estimate = 13 }) as ObjectResult;

            Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Active Item not found", result.Value);
        }

        [Fact]
        public async Task Vote_given_nonActive_round_returns_notFound()
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

            var controller = new SessionController(sessionRepo.Object, cache, null);

            var result = await controller.Vote(token, "ABC1234", new VoteCreateUpdateDTO { Estimate = 13 }) as ObjectResult;

            Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Active Round not found", result.Value);
        }

        [Fact]
        public async Task Vote_given_active_item_and_round_gives_vote()
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
                                Votes = new List<VoteDTO> { new VoteDTO { Estimate = 5 } }
                            }
                        }
                    }
                },
                SessionKey = "ABC1234",
                Users = new List<UserDTO> { new UserDTO(), new UserDTO() }
            };

            sessionRepo.Setup(s => s.FindByKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(mockSession);

            var controller = new SessionController(sessionRepo.Object, cache, null);

            var result = await controller.Vote(token, "ABC1234", new VoteCreateUpdateDTO { Estimate = 13 });

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void WhoAmI_given_invalid_token_and_sessionKey_return_unauthorized()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());

            var controller = new SessionController(null, cache, null);

            var result = controller.WhoAmI("Idontexist", "neitherdoi");

            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        [Fact]
        public void WhoAmI_given_valid_token_and_sessionkey_return_userState()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());

            var token = CreateUserState(cache, 42, "ABC1234");

            var controller = new SessionController(null, cache, null);

            var result = controller.WhoAmI(token, "ABC1234");

            Assert.IsType<UserState>(result.Value);
            Assert.Equal(42, result.Value.Id);
            Assert.Equal("ABC1234", result.Value.SessionKey);
        }

        private static string CreateUserState(IMemoryCache cache, int userId, string sessionKey)
        {
            var sm = new UserStateManager(cache);
            return sm.CreateState(userId, sessionKey);
        }
    }
}
