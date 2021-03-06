namespace PlanningPoker.App.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Identity.Client;
    using Xamarin.Forms;

    public class BearerTokenClientHandler : HttpClientHandler
    {
        private readonly IPublicClientApplication publicClientApplication;
        private readonly IReadOnlyCollection<string> scopes;
        private readonly ISettings settings;

        public BearerTokenClientHandler(IPublicClientApplication publicClientApplication, ISettings settings)
        {
            this.publicClientApplication = publicClientApplication;
            this.scopes = settings.Scopes;
            this.settings = settings;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accounts = await this.publicClientApplication.GetAccountsAsync();

            var firstAccount = accounts.FirstOrDefault();

            if (firstAccount != null)
            {
                var result = await this.publicClientApplication.AcquireTokenSilentAsync(this.scopes, firstAccount);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            }

            if (Application.Current.Properties.ContainsKey("token"))
            {
                request.Headers.Add("PPAuthorization", Application.Current.Properties["token"] as string);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
