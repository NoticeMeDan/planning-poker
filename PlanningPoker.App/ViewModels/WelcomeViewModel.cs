namespace PlanningPoker.App.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Microsoft.Identity.Client;
    using Models;
    using PlanningPoker.Shared;
    using Xamarin.Forms;

    public class WelcomeViewModel : BaseViewModel
    {
        private readonly IPublicClientApplication publicClientApplication;

        private readonly ISettings settings;

        private bool loading;

        public bool Connection { get; set; }

        private readonly ISessionClient client;

        public UserCreateDTO User { get; set; }

        private string nickname;

        private string key;

        public ICommand JoinCommand { get; }

        public ICommand LoginCommand { get; }

        public WelcomeViewModel(IPublicClientApplication publicClientApplication, ISettings settings, ISessionClient client)
        {
            this.publicClientApplication = publicClientApplication;
            this.client = client;
            this.settings = settings;
            this.BaseTitle = "Login";
            this.LoginCommand = new Command(async () => await this.ExecuteLoginCommand());
            this.User = this.CreateGuestUserDTO();
            this.JoinCommand = new RelayCommand(_ => this.ExecuteJoinCommand());
        }

        public async Task<bool> ExecuteLoginCommand()
        {
            AuthenticationResult authenticationResult = null;
            IEnumerable<IAccount> accounts = await this.publicClientApplication.GetAccountsAsync();
            try
            {
                IAccount account = accounts.FirstOrDefault();
                authenticationResult =
                    await this.publicClientApplication.AcquireTokenSilentAsync(this.settings.Scopes, account);
                return true;
            }
            catch (MsalUiRequiredException e)
            {
                authenticationResult =
                    await this.publicClientApplication.AcquireTokenAsync(this.settings.Scopes, App.UiParent);
                var message = e.StackTrace;
                return true;
            }
            catch (Exception e)
            {
                var message = e.Message;
                return false;
            }
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
                var x = await this.client.Join(this.key, this.User);
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
                SessionClient repoMock = (SessionClient)this.client;
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
