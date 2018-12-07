namespace PlanningPoker.WebApi.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    public class LobbyHub : Hub
    {
        public async Task SendJoined(int id, string name)
        {
            await Clients.All.SendAsync("UserJoined", id);
        }
    }
}
