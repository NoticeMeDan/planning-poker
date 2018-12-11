namespace PlanningPoker.App.Models
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Shared;

    public class SessionRepository : ISessionRepository
    {
        private readonly HttpClient httpClient;

        public SessionRepository(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<SessionDTO> CreateAsync(SessionCreateUpdateDTO session)
        {
            var response = await this.httpClient.PostAsJsonAsync("api/session", session);

            return await response.Content.ReadAsAsync<SessionDTO>();
        }

        public async Task<SessionDTO> FindAsync(int sessionId)
        {
            var response = await this.httpClient.GetAsync($"api/session/{sessionId}");

            return await response.Content.ReadAsAsync<SessionDTO>();
        }

        public async Task<IEnumerable<SessionDTO>> ReadAsync()
        {
            var response = await this.httpClient.GetAsync("api/session");

            return await response.Content.ReadAsAsync<IEnumerable<SessionDTO>>();
        }

        public async Task<bool> UpdateAsync(SessionCreateUpdateDTO session)
        {
            var response = await this.httpClient.PutAsJsonAsync($"api/session/{session.Id}", session);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int sessionId)
        {
            var response = await this.httpClient.DeleteAsync($"api/session/{sessionId}");

            return response.IsSuccessStatusCode;
        }
    }
}
