namespace PlanningPoker.App.Tests.ViewModelsTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Moq;
    using PlanningPoker.App.Models;
    using PlanningPoker.App.ViewModels;
    using PlanningPoker.Shared;
    using Xunit;

    public class LobbyViewModelTests
    {
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

            Assert.Equal(session.Items.Count, lobbyViewModel.Items.Count);
        }
    }
}
