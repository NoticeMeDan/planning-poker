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

        public bool Loading { get; set; }

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
            this.JoinCommand = new RelayCommand(async _ => await this.ExecuteJoinCommand());
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
                //TODO: Save token in this.settings.token
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
                Nickname = "Guest"
            };
        }

        private async Task ExecuteJoinCommand()
        {
            if (this.Loading)
            {
                return;
            }

            this.Loading = true;

            this.User.Nickname = this.nickname;

            await this.JoinSession();

            this.Loading = false;
        }

        private async Task JoinSession()
        {
            try
            {
                var x = await this.client.Join(this.key, this.User);
                //TODO: Save token in this.settings.token = x.Token ?
                Debug.Write("User TOKEN: " + x.Token);
            }
            catch (Exception e)
            {
                this.Key = string.Empty;
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
