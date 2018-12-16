namespace PlanningPoker.App.ViewModels
{
    using System.Diagnostics;
    using System.Windows.Input;
    using Interfaces;

    /*
     * Waiting for api until implementation
     * what you will see is pseudocode
     */
    public class JoinViewModel : BaseViewModel, IJoinViewModel
    {
        public JoinViewModel()
        {
            this.BaseTitle = "Join";

            this.JoinLobbyCommand = new RelayCommand(_ => this.JoinLobby());
        }

        private ICommand JoinLobbyCommand { get; }

        public void JoinLobby()
        {
        }

        public void JoinLobby(int key)
        {
            Debug.WriteLine("Connection!");

            // Connect to database and find session
        }

        public bool KeyExist(int key)
        {
            // Call the api to see if key exist and return
            // return (key == this.testKey) ? true : false;
            return true;
        }
    }
}
