namespace PlanningPoker.App.ViewModels
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using PlanningPoker.App.Models;
    using PlanningPoker.App.Views.Session;
    using PlanningPoker.Shared;

    public class JoinViewModel : BaseViewModel
    {

        private bool Loading;

        public bool connection { get; set; }

        private readonly ISessionRepository repository;

        public UserCreateDTO User { get; set; }

        private string nickname;

        private string key;

        public ICommand JoinCommand { get; }

        public JoinViewModel(ISessionRepository repository)
        {
            this.User = new UserCreateDTO
            {
                IsHost = false,
                Email = string.Empty,
                Nickname = "Guest"
            };

            this.repository = repository;
            this.JoinCommand = new RelayCommand(async _ => await this.ExecuteJoinCommand());
        }

        private async Task ExecuteJoinCommand()
        {
            if (this.Loading)
            {
                return;
            }

            this.Loading = true;
            this.connection = false;
            this.User.Nickname = this.nickname;

            try
            {
                var x = await this.repository.Join(this.key, this.User);
                Debug.Write("User TOKEN: " + x.Token);
                this.connection = true;
            }
            catch (Exception e)
            {
                this.connection = false;
                Debug.WriteLine("No session with that key exists.");
                Debug.WriteLine(e.StackTrace);
            }

            this.Key = string.Empty;
            this.Loading = false;
        }

        public string Nickname
        {
            get => this.nickname;
            set => this.SetProperty(ref this.nickname, value);
        }

        public string Key
        {
            get => this.key;
            set => this.SetProperty(ref this.key, value);
        }
    }
}
