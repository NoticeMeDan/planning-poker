namespace PlanningPoker.App.Models
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Shared;

    public class SummaryClient : ISummaryClient
    {
        private readonly HttpClient httpClient;

        public SummaryClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<SummaryDTO> FindBySessionIdAsync(int sessionId)
        {
            var response = await this.httpClient.GetAsync($"api/session/{sessionId}");
            var result = JsonConvert.DeserializeObject<SummaryDTO>(response.Content.ReadAsStringAsync().Result);

            return result;
        }
    }
}
