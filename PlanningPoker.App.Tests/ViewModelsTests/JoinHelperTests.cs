namespace PlanningPoker.App.Tests.ViewModelsTests
{
    using System.Threading.Tasks;
    using Moq;
    using PlanningPoker.App.Models;
    using PlanningPoker.App.ViewModels;
    using PlanningPoker.Shared;
    using Xunit;

    public class JoinHelperTests
    {
        [Fact]
        public async Task JoinSession_sets_token()
        {
            var client = new Mock<ISessionClient>();

            var token = new UserStateResponseDTO
            {
                Token = "AuthenticationToken12345"
            };
            var sessionKey = "1234567";
            var user = new UserCreateDTO
            {
                Nickname = "Test",
                IsHost = false,
            };

            client.Setup(s => s.Join(sessionKey, user)).ReturnsAsync(token);

            var joinHelper = new JoinHelper(client.Object, sessionKey, user);

            await joinHelper.JoinSession();

            Assert.Equal(token.Token, joinHelper.Token);
        }
    }
}
