using PlanningPoker.App.Models;
using PlanningPoker.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlanningPoker.App.ViewModels
{
    class LobbyViewModel : BaseViewModel
    {

        private bool Loading;

        public bool connection { get; set; }

        private readonly ISessionRepository repository;

        public UserDTO User { get; set; }

        public ICommand getUsersCommand { get; }

        public LobbyViewModel(ISessionRepository repository)
        {
            this.repository = repository;
            this.getUsersCommand = new RelayCommand(async _ => await this.ExecuteGetUsersCommand());
        }

        private async Task ExecuteGetUsersCommand()
        {
            if (this.Loading)
            {
                return;
            }

            this.Loading = true;

            this.Loading = false;
        }
    }
}
