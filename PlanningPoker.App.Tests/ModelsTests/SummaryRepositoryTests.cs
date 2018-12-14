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

    public class SummaryRepositoryTests
    {
        private readonly Uri baseAddress = new Uri("https://localhost:5001/");

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

            var repository = new SummaryRepository(client);

            await repository.FindAsync(42);

            handler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri("https://localhost:5001/api/summaries/42")),
                ItExpr.IsAny<CancellationToken>());
        }
    }
}
