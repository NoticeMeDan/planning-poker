namespace PlanningPoker.WebApi.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    public class VotesHub : Hub
    {
        public async Task SendVote(string user, int voteValue)
        {
            await Clients.All.SendAsync("UserVote", user, voteValue);
        }
    }
}
