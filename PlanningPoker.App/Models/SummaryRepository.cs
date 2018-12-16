using Newtonsoft.Json;

namespace PlanningPoker.App.Models
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Shared;

    public class SummaryRepository : ISummaryRepository
    {
        private readonly HttpClient httpClient;

        public SummaryRepository(HttpClient httpClient)
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
