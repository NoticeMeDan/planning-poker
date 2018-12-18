namespace PlanningPoker.App.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Identity.Client;

    public class BearerTokenClientHandler : HttpClientHandler
    {
        private readonly IPublicClientApplication publicClientApplication;
        private readonly IReadOnlyCollection<string> scopes;
        private ISettings settings;

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

            request.Headers.Add("PPAuthorization", "Bearer 4a83c928-5c40-4dc9-9567-c45df78514ea");
            /*request.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "Your Oauth token");*/

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
