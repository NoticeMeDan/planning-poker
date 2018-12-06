using PlanningPoker.Shared;

namespace PlanningPoker.App.Tests
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using System;
    using System.Net;
    using System.Threading;
    using Xunit;
    using Moq;
    using Moq.Protected;
    using Models;

    public class SummaryRepositoryTests
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
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Created,
                    Content = new StringContent("")
                })
                .Verifiable();

            var client = new HttpClient(handler.Object)
            {
                BaseAddress = baseAddress
            };

            var repository = new SummaryRepository(client);

            var dto = new SummaryCreateUpdateDTO
            {
                Id = 42,
                SessionId = 42
            };

            await repository.CreateAsync(dto);

            handler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post
                    && req.RequestUri == new Uri("https://localhost:5001/api/summaries")
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task FindAsync_sends_ok()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("")
                });

            var client = new HttpClient(handler.Object)
            {
                BaseAddress = baseAddress
            };

            var repository = new SummaryRepository(client);

            await repository.FindAsync(42);

            handler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri("https://localhost:5001/api/summaries/42")
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task ReadAsync_sends_ok()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("")
                });

            var client = new HttpClient(handler.Object)
            {
                BaseAddress = baseAddress
            };

            var repository = new SummaryRepository(client);

            await repository.ReadAsync();

            handler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri("https://localhost:5001/api/summaries")
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task UpdateAsync_sends_ok()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("")
                });

            var client = new HttpClient(handler.Object)
            {
                BaseAddress = baseAddress
            };

            var repository = new SummaryRepository(client);

            var dto = new SummaryCreateUpdateDTO
            {
                Id = 42,
                SessionId = 42
            };

            await repository.UpdateAsync(dto);

            handler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Put
                    && req.RequestUri == new Uri("https://localhost:5001/api/summaries/42")
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task DeleteAsync_sends_delete()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent
                });

            var client = new HttpClient(handler.Object)
            {
                BaseAddress = baseAddress
            };

            var repository = new SummaryRepository(client);

            var result = await repository.DeleteAsync(42);

            Assert.True(result);

            handler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Delete
                    && req.RequestUri == new Uri("https://localhost:5001/api/summaries/42")
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}
