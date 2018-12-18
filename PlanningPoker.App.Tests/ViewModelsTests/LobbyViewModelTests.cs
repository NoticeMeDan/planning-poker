namespace PlanningPoker.App.Tests.ViewModelsTests
{
    using System.Collections.Generic;
    using Moq;
    using PlanningPoker.App.Models;
    using PlanningPoker.App.ViewModels;
    using PlanningPoker.Shared;
    using Xunit;

    public class LobbyViewModelTests
    {
        [Fact]
        private void FetchUsers_fetches_session_and_calls_UpdateUserCollection_and_updates_Users()
        {
            var client = new Mock<ISessionClient>();

            var users = new List<UserDTO>
            {
                new UserDTO
                {
                    Nickname = "Test",
                    IsHost = false
                }
            };

            var session = new SessionDTO
            {
                Users = users,
                SessionKey = "1234567"
            };

            client.Setup(s => s.GetByKeyAsync(session.SessionKey)).ReturnsAsync(session);

            var lobbyViewModel = new LobbyViewModel(client.Object);

            lobbyViewModel.UpdateUserCollection(session.Users);

            Assert.Equal(session.Users, lobbyViewModel.Users);
        }

        [Fact]
        private void FetchUsers_fetches_session_and_calls_UpdateItemCollection_and_updates_Items()
        {
            var client = new Mock<ISessionClient>();

            var items = new List<ItemDTO>
            {
                new ItemDTO
                {
                    Title = "Test",
                    Description = "Test"
                }
            };

            var session = new SessionDTO
            {
                Items = items,
                SessionKey = "1234567"
            };

            client.Setup(s => s.GetByKeyAsync(session.SessionKey)).ReturnsAsync(session);

            var lobbyViewModel = new LobbyViewModel(client.Object);

            lobbyViewModel.UpdateItemCollection(session.Items);

            Assert.Equal(session.Items, lobbyViewModel.Items);
        }
    }
}
