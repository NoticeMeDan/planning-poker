namespace PlanningPoker.App.ViewModels
{
    using System;
    using System.Collections.Generic;
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

        private readonly ISessionClient client;

        public UserCreateDTO User { get; set; }

        private string nickname;

        private string key;

        private JoinHelper joinCommander;

        public ICommand Join { get; }

        public ICommand LoginCommand { get; }

        public WelcomeViewModel(IPublicClientApplication publicClientApplication, ISettings settings, ISessionClient client)
        {
            this.publicClientApplication = publicClientApplication;
            this.client = client;
            this.settings = settings;
            this.BaseTitle = "Login";
            this.LoginCommand = new Command(async () => await this.ExecuteLoginCommand());
            this.User = this.CreateGuestUserDTO();
            this.Join = new RelayCommand(_ => this.ExecuteJoinCommand());
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
                Nickname = "Guest"
            };
        }

        private void ExecuteJoinCommand()
        {
            this.User.Nickname = this.nickname;

            this.joinCommander = new JoinHelper(this.client, this.key, this.User);

            this.joinCommander.Join.Execute(null);

            this.Key = this.joinCommander.Key;

            this.settings.Token = this.joinCommander.Token;
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
