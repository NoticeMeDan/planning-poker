
using Microsoft.AspNetCore.SignalR.Client;

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


        public JoinViewModel()
        {
            //create hub connection
            this.hubConnection = new HubConnectionBuilder()
                .WithUrl("localhost:4000")   //TODO hub URL
                .Build();

            //
            this.hubConnection.On<string>("UserJoined", (message) =>
            {
                //TODO handle message
            });

            hubConnection.StartAsync();
        }


        /*
         * Returns allowance
         */
        public void JoinLobby(string key)
        {
            Debug.WriteLine("Connection!");

            // Connect to database and find session

            //Connect to Hub
        }

        private bool KeyExist(string key)
        {
            // Call the api to see if key exist and return
            return (key == this.testKey) ? true : false;
        }
    }
}
