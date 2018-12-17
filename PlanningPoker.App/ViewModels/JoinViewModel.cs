namespace PlanningPoker.App.ViewModels
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using PlanningPoker.App.Models;
    using PlanningPoker.Shared;

    public class JoinViewModel : BaseViewModel
    {
        private bool loading;

        public bool Connection { get; set; }

        private readonly ISessionRepository repository;

        public UserCreateDTO User { get; set; }

        private string nickname;

        private string key;

        public ICommand JoinCommand { get; }

        public JoinViewModel(ISessionRepository repository)
        {
            this.User = this.CreateGuestUserDTO();
            this.repository = repository;
            this.JoinCommand = new RelayCommand(_ => this.ExecuteJoinCommand());
        }

        private UserCreateDTO CreateGuestUserDTO()
        {
            return new UserCreateDTO
            {
                IsHost = false,
                Email = string.Empty,
                Nickname = "Guest"
            };
        }

        private void ExecuteJoinCommand()
        {
            if (this.loading)
            {
                return;
            }

            this.loading = true;
            this.Connection = false;

            this.User.Nickname = this.nickname;

            this.JoinSessionMock();

            this.loading = false;
        }

        private async Task JoinSession()
        {
            try
            {
                var x = await this.repository.Join(this.key, this.User);
                Debug.Write("User TOKEN: " + x.Token);
                this.Connection = true;
            }
            catch (Exception e)
            {
                this.Connection = false;
                Debug.WriteLine("No session with that key exists.");
                Debug.WriteLine(e.StackTrace);
            }
        }

        private void JoinSessionMock()
        {
            try
            {
                SessionRepository repoMock = (SessionRepository)this.repository;
                repoMock.JoinMock(this.key, this.User);
                this.Connection = true;
            }
            catch (Exception e)
            {
                this.Connection = false;
                Debug.WriteLine("No session with that key exists.");
                Debug.WriteLine(e.StackTrace);
            }
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
