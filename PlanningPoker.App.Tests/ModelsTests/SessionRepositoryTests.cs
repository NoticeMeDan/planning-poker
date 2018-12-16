/*
namespace PlanningPoker.App.Tests.ModelsTests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;
    using Moq;
    using Moq.Protected;
    using Shared;
    using Xunit;

    public class SessionRepositoryTests
    {
        private readonly Uri baseAddress = new Uri("https://localhost:5001/");

        [Fact]
        public async Task CreateAsync_sends_created()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Created,
                    Content = new StringContent(string.Empty)
                })
                .Verifiable();

            var client = new HttpClient(handler.Object)
            {
                BaseAddress = this.baseAddress
            };

            var repository = new SessionRepository(client);

            var dto = new SessionCreateUpdateDTO
            {
                Id = 42,
            };

            await repository.CreateAsync(dto);

            handler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post
                    && req.RequestUri == new Uri("https://localhost:5001/api/session")),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task FindAsync_sends_ok()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(string.Empty)
                });

            var client = new HttpClient(handler.Object)
            {
                BaseAddress = this.baseAddress
            };

            var repository = new SessionRepository(client);

            await repository.FindAsync(42);

            handler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri("https://localhost:5001/api/session/42")),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task UpdateAsync_sends_ok()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(string.Empty)
                });

            var client = new HttpClient(handler.Object)
            {
                BaseAddress = this.baseAddress
            };

            var repository = new SessionRepository(client);

            var dto = new SessionCreateUpdateDTO
            {
                Id = 42,
            };

            await repository.UpdateAsync(dto);

            handler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Put
                    && req.RequestUri == new Uri("https://localhost:5001/api/session/42")),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetByKeyAsync_sends_ok()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(string.Empty)
                });

            var client = new HttpClient(handler.Object)
            {
                BaseAddress = this.baseAddress
            };

            var repository = new SessionRepository(client);

            var sessionKey = "42";

            await repository.GetByKeyAsync(sessionKey);

            handler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri("https://localhost:5001/api/session/42")),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task Join_sends_created()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(string.Empty)
                });

            var client = new HttpClient(handler.Object)
            {
                BaseAddress = this.baseAddress
            };

            var repository = new SessionRepository(client);

            var sessionKey = "42";

            var user = new UserCreateDTO
            {
                Id = 42
            };

            await repository.Join(sessionKey, user);

            handler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post
                    && req.RequestUri == new Uri("https://localhost:5001/api/session/42/join")),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task NextRoundAsync_sends_ok()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(string.Empty)
                });

            var client = new HttpClient(handler.Object)
            {
                BaseAddress = this.baseAddress
            };

            var repository = new SessionRepository(client);

            var sessionKey = "42";

            await repository.NextRoundAsync(sessionKey);

            handler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri("https://localhost:5001/api/session/42/item/round/next")),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetCurrentRound_sends_ok()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(string.Empty)
                });

            var client = new HttpClient(handler.Object)
            {
                BaseAddress = this.baseAddress
            };

            var repository = new SessionRepository(client);

            var sessionKey = "42";

            await repository.GetCurrentRound(sessionKey);

            handler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri("https://localhost:5001/api/session/42/item/round")),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task NextItemAsync_sends_ok()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(string.Empty)
                });

            var client = new HttpClient(handler.Object)
            {
                BaseAddress = this.baseAddress
            };

            var repository = new SessionRepository(client);

            var sessionKey = "42";

            await repository.NextItemAsync(sessionKey);

            handler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri("https://localhost:5001/api/session/42/item/next")),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetAllItems_sends_ok()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(string.Empty)
                });

            var client = new HttpClient(handler.Object)
            {
                BaseAddress = this.baseAddress
            };

            var repository = new SessionRepository(client);

            var sessionKey = "42";

            await repository.GetAllItems(sessionKey);

            handler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri("https://localhost:5001/api/session/42/item")),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetCurrentItem_sends_ok()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(string.Empty)
                });

            var client = new HttpClient(handler.Object)
            {
                BaseAddress = this.baseAddress
            };

            var repository = new SessionRepository(client);

            var sessionKey = "42";

            await repository.GetCurrentItem(sessionKey);

            handler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri("https://localhost:5001/api/session/42/item/current")),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task Vote_sends_ok()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(string.Empty)
                });

            var client = new HttpClient(handler.Object)
            {
                BaseAddress = this.baseAddress
            };

            var repository = new SessionRepository(client);

            var sessionKey = "42";

            var vote = new VoteDTO
            {
                Id = 42
            };

            await repository.Vote(sessionKey, vote);

            handler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post
                    && req.RequestUri == new Uri("https://localhost:5001/api/session/42/vote")),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task ThrowNitpickerCard_sends_ok()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(string.Empty)
                });

            var client = new HttpClient(handler.Object)
            {
                BaseAddress = this.baseAddress
            };

            var repository = new SessionRepository(client);

            var sessionKey = "42";

            await repository.ThrowNitpickerCard(sessionKey);

            handler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post
                    && req.RequestUri == new Uri("https://localhost:5001/api/session/42/nitpicker")),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task KickUser_sends_ok()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(string.Empty)
                });

            var client = new HttpClient(handler.Object)
            {
                BaseAddress = this.baseAddress
            };

            var repository = new SessionRepository(client);

            var sessionKey = "42";

            var user = new UserDTO
            {
                Id = 42
            };

            await repository.KickUser(sessionKey, user.Id);

            handler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post
                    && req.RequestUri == new Uri("https://localhost:5001/api/session/42/user/kick")),
                ItExpr.IsAny<CancellationToken>());
        }
    }
}
*/
