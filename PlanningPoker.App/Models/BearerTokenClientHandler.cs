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
        private readonly string token;

        public BearerTokenClientHandler(IPublicClientApplication publicClientApplication, ISettings settings)
        {
            this.publicClientApplication = publicClientApplication;
            this.scopes = settings.Scopes;
            this.token = settings.Token;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accounts = await this.publicClientApplication.GetAccountsAsync();

            var result = await this.publicClientApplication.AcquireTokenSilentAsync(this.scopes, accounts.First());

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            request.Headers.Add("PPAuthentication", this.token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
