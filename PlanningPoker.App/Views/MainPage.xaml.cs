using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using PlanningPoker.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanningPoker.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

        }

        async void OnSignInSignOut(object sender, EventArgs e)
        {
            var settings = new Settings();
            AuthenticationResult authResult = null;
            IEnumerable<IAccount> accounts = await App.publicClientApplication.GetAccountsAsync();
            try
            {
                if (btnSignInSignOut.Text == "Sign in")
                {
                    // let's see if we have a user in our belly already
                    try
                    {
                        IAccount firstAccount = accounts.FirstOrDefault();
                        authResult = await App.publicClientApplication.AcquireTokenSilentAsync(settings.Scopes, firstAccount);
                        await RefreshUserDataAsync(authResult.AccessToken).ConfigureAwait(false);
                        Device.BeginInvokeOnMainThread(() => { btnSignInSignOut.Text = "Sign out"; });
                    }
                    catch (MsalUiRequiredException ex)
                    {
                        authResult = await App.publicClientApplication.AcquireTokenAsync(settings.Scopes, App.UiParent);
                        await RefreshUserDataAsync(authResult.AccessToken);
                        Device.BeginInvokeOnMainThread(() => { btnSignInSignOut.Text = "Sign out"; });
                    }
                }
                else
                {
                    while (accounts.Any())
                    {
                        await App.publicClientApplication.RemoveAsync(accounts.FirstOrDefault());
                        accounts = await App.publicClientApplication.GetAccountsAsync();
                    }

                    slUser.IsVisible = false;
                    Device.BeginInvokeOnMainThread(() => { btnSignInSignOut.Text = "Sign in"; });
                }
            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();
            }
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

                slUser.IsVisible = true;

                Device.BeginInvokeOnMainThread(() =>
                {

                    lblDisplayName.Text = user["displayName"].ToString();
                    lblGivenName.Text = user["givenName"].ToString();
                    lblId.Text = user["id"].ToString();
                    lblSurname.Text = user["surname"].ToString();
                    lblUserPrincipalName.Text = user["userPrincipalName"].ToString();

                    // just in case
                    btnSignInSignOut.Text = "Sign out";
                });
            }
            else
            {
                await DisplayAlert("Something went wrong with the API call", responseString, "Dismiss");
            }
        }
    }
}