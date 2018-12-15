namespace PlanningPoker.App.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using PlanningPoker.App.Models;
    using PlanningPoker.Shared;

    public class JoinViewModel
    {
        private readonly ISessionRepository repository;

        public UserCreateDTO user { get; set; }

        public JoinViewModel(ISessionRepository repository)
        {
            this.user = new UserCreateDTO
            {
                IsHost = false,
                Email = string.Empty,
                Nickname = "Guest"
            };

            this.repository = repository;
        }

        public async void JoinLobby(string key)
        {
            Debug.WriteLine("Connection!");

            try
            {
                await this.repository.Join(key, this.user);
            }
            catch (KeyNotFoundException e)
            {
                Debug.WriteLine("No session with that key exists.");
                Debug.WriteLine(e.StackTrace);
            }
        }
    }
}
