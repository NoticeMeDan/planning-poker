namespace PlanningPoker.App.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Microsoft.Identity.Client;
    using Newtonsoft.Json.Linq;
    using PlanningPoker.App.Models;

    public class LoginViewModel : BaseViewModel
    {
        public ICommand LoginCommand { get; set; }

        public async void ExecuteLoginCommand()
        {
            var settings = new Settings();
            AuthenticationResult authResult = null;
            IEnumerable<IAccount> accounts = await App.publicClientApplication.GetAccountsAsync();
            try
            {
                IAccount firstAccount = accounts.FirstOrDefault();
                authResult = await App.publicClientApplication.AcquireTokenSilentAsync(settings.Scopes, firstAccount);
                await this.RefreshUserDataAsync(authResult.AccessToken).ConfigureAwait(false);
                // Device.BeginInvokeOnMainThread(() => { });
            }
            catch (MsalUiRequiredException ex)
            {
                // authResult = await App.publicClientApplication.AcquireTokenWithDeviceCodeAsync(settings.Scopes, App.UiParent);
                await this.RefreshUserDataAsync(authResult.AccessToken);
                // TODO: Redirect to new page
                // Device.BeginInvokeOnMainThread(() => { });
            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();
            }
        }

        public async Task RefreshUserDataAsync(string token)
        {
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
