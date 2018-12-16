using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace PlanningPoker.App.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Microsoft.Identity.Client;
    using Models;
    using Xamarin.Forms;

    public class WelcomeViewModel : BaseViewModel
    {
        private readonly IPublicClientApplication publicClientApplication;
        private readonly ISettings settings;
        private string username;

        public WelcomeViewModel(IPublicClientApplication publicClientApplication, ISettings settings)
        {
            this.publicClientApplication = publicClientApplication;
            this.settings = settings;
            this.BaseTitle = "Login";
            this.LoginCommand = new Command(async () => await this.ExecuteLoginCommand());
            this.JoinCommand = new Command(async () => await this.ExecuteJoinCommand());
        }

        public ICommand LoginCommand { get; }

        public ICommand JoinCommand { get; }

        public string Username
        {
            get => this.username;
            set => this.SetProperty(ref this.username, value);
        }

        public async Task ExecuteJoinCommand()
        {
            Debug.WriteLine("Joined lobby!");
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
    }
}
