namespace PlanningPoker.App.Models
{
    using System.Collections.Generic;
    using System.Net;
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

        public async Task<SessionDTO> GetByKeyAsync(string sessionKey)
        {
            var reponse = await this.httpClient.GetAsync($"api/session/{sessionKey}");

            return await reponse.Content.ReadAsAsync<SessionDTO>();
        }

        public async Task<UserStateResponseDTO> Join(string sessionKey, UserCreateDTO user)
        {
            var response = await this.httpClient.PostAsJsonAsync($"api/session/{sessionKey}/join", user);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new KeyNotFoundException();
            }

            return await response.Content.ReadAsAsync<UserStateResponseDTO>();
        }

        public async Task<RoundDTO> NextRoundAsync(string sessionKey)
        {
            var response = await this.httpClient.GetAsync($"api/session/{sessionKey}/item/round/next");

            return await response.Content.ReadAsAsync<RoundDTO>();
        }

        public async Task<RoundDTO> GetCurrentRound(string sessionKey)
        {
            var response = await this.httpClient.GetAsync($"api/session/{sessionKey}/item/round");

            return await response.Content.ReadAsAsync<RoundDTO>();
        }

        public async Task<ItemDTO> NextItemAsync(string sessionKey)
        {
            var response = await this.httpClient.GetAsync($"api/session/{sessionKey}/item/next");

            return await response.Content.ReadAsAsync<ItemDTO>();
        }

        public async Task<ICollection<ItemDTO>> GetAllItems(string sessionKey)
        {
            var response = await this.httpClient.GetAsync($"api/session/{sessionKey}/item");

            return await response.Content.ReadAsAsync<ICollection<ItemDTO>>();
        }

        public async Task<ItemDTO> GetCurrentItem(string sessionKey)
        {
            var response = await this.httpClient.GetAsync($"api/session/{sessionKey}/item/current");

            return await response.Content.ReadAsAsync<ItemDTO>();
        }

        public async Task<bool> Vote(string sessionKey, VoteDTO vote)
        {
            var response = await this.httpClient.PostAsJsonAsync($"api/session/{sessionKey}/vote", vote);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ThrowNitpickerCard(string sessionKey)
        {
            var response = await this.httpClient.PostAsync($"api/session/{sessionKey}/nitpicker", default(HttpContent));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> KickUser(string sessionKey, int userId)
        {
            var response = await this.httpClient.PostAsJsonAsync($"api/session/{sessionKey}/user/kick", userId);

            return response.IsSuccessStatusCode;
        }
    }
}
