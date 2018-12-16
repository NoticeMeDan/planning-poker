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

        public async Task<SummaryDTO> FindAsync(int summaryId)
        {
            var response = await this.httpClient.GetAsync($"api/summaries/{summaryId}");

            return await response.Content.ReadAsAsync<SummaryDTO>();
        }
    }
}
