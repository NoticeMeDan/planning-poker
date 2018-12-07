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
    using Newtonsoft.Json.Linq;
    using Xamarin.Forms;

    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            this.LoginCommand = new Command(async () => await this.ExecuteLoginCommand());
        }

        public ICommand LoginCommand { get; }

         public async Task<bool> ExecuteLoginCommand()
        {
            var settings = new Settings();
            AuthenticationResult authResult = null;
            IEnumerable<IAccount> accounts = await App.GetPublicClientApplication().GetAccountsAsync();
                    try
                    {
                        IAccount firstAccount = accounts.FirstOrDefault();
                        authResult = await App.GetPublicClientApplication().AcquireTokenSilentAsync(settings.Scopes, firstAccount);
                        await this.RefreshUserDataAsync(authResult.AccessToken).ConfigureAwait(false);
                        return true;
                    }
                    catch (MsalUiRequiredException ex)
                    {
                        authResult = await App.GetPublicClientApplication().AcquireTokenAsync(settings.Scopes, App.UiParent);
                        await this.RefreshUserDataAsync(authResult.AccessToken);

                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
            return false;
        }

        public async Task RefreshUserDataAsync(string token)
        {
            //get data from API
            HttpClient client = new HttpClient();
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me");
            message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
            HttpResponseMessage response = await client.SendAsync(message);
            string responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                JObject user = JObject.Parse(responseString);
            }
        }
    }
}
