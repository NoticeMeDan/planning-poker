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
            var response = await this.httpClient.PostAsJsonAsync("api/summaries", summary);

            return await response.Content.ReadAsAsync<SummaryDTO>();
        }

        public async Task<SummaryDTO> FindAsync(int summaryId)
        {
            var response = await this.httpClient.GetAsync($"api/summaries/{summaryId}");

            return await response.Content.ReadAsAsync<SummaryDTO>();
        }

        public async Task<IEnumerable<SummaryDTO>> ReadAsync()
        {
            var response = await this.httpClient.GetAsync("api/summaries");

            return await response.Content.ReadAsAsync<IEnumerable<SummaryDTO>>();
        }

        public async Task<bool> UpdateAsync(SummaryCreateUpdateDTO summary)
        {
            var response = await this.httpClient.PutAsJsonAsync($"api/summaries/{summary.Id}", summary);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int summaryId)
        {
            var response = await this.httpClient.DeleteAsync($"api/summaries/{summaryId}");

            return response.IsSuccessStatusCode;
        }
    }
}
