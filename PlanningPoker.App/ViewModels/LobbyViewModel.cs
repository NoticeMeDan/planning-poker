namespace PlanningPoker.App.ViewModels {
    using System.Windows.Input;
    using Interfaces;

    public class LobbyViewModel : BaseViewModel, ILobbyViewModel
    {
        public ICommand LoadCommand { get; private set; }

        public LobbyViewModel()
        {
            this.BaseTitle = "Lobby";

            this.LoadCommand = new RelayCommand(_ => this.ExecuteLoadCommand());
        }

        private void ExecuteLoadCommand()
        {
        }
    }
}
