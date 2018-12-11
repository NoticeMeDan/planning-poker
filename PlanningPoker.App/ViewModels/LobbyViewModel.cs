namespace PlanningPoker.App.ViewModels
{
    using System.Windows.Input;
    using Interfaces;

    public class LobbyViewModel : BaseViewModel, ILobbyViewModel
    {
        public LobbyViewModel()
        {
            this.BaseTitle = "Lobby";

            this.LoadCommand = new RelayCommand(_ => this.ExecuteLoadCommand());
        }

        public ICommand LoadCommand { get; private set; }

        private void ExecuteLoadCommand()
        {
        }
    }
}
