
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PlanningPoker.App.Models;

namespace PlanningPoker.App.ViewModels
{
    using System;
    using System.Diagnostics;

    /*
     * Waiting for api until implementation
     * what you will see is pseudocode
     */
    public class JoinViewModel
    {
        private string testKey = "1234567";

        private HubConnection hubConnection;
        private Settings settings = new Settings();


        public JoinViewModel()
        {
            //create hub connection
            this.hubConnection = new HubConnectionBuilder()
                .WithUrl(settings.BackendUrl + "lobbyHub")
                .Build();

            //
            this.hubConnection.On<string>("UserJoined", (id) =>
            {
                Debug.WriteLine("Received UserJoined: " + id);
            });
        }

        public async Task startConnection()
        {
            await hubConnection.StartAsync();
        }

        /*
         * Returns allowance
         */
        public async Task JoinLobby(string key)
        {
            Debug.WriteLine("Connection!");

            // Connect to database and find session

            //Connect to Hub
            await this.startConnection();
            await this.hubConnection.InvokeAsync("SendJoined", 42);
        }

        private bool KeyExist(string key)
        {
            // Call the api to see if key exist and return
            return (key == this.testKey) ? true : false;
        }
    }
}
