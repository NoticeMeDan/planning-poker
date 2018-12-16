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

        public async Task<SummaryDTO> CreateAsync(SummaryCreateUpdateDTO summary)
        {
            var response = await this.httpClient.PostAsJsonAsync("api/summary", summary);

            return await response.Content.ReadAsAsync<SummaryDTO>();
        }

        public async Task<SummaryDTO> FindAsync(int summaryId)
        {
            var response = await this.httpClient.GetAsync($"api/summaries/{summaryId}");

            return await response.Content.ReadAsAsync<SummaryDTO>();
        }
        /*
        public async Task<SummaryDTO> FindBySessionIdAsync(int sessionId)
        {
            var sessionresponse = this.httpClient.GetAsync($"api/session/{sessionId}");
            var summaryId = await this.httpClient.GetAsync

        }*/
    }
}
