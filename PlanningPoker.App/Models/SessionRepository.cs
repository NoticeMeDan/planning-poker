namespace PlanningPoker.App.Models
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Shared;

    public class SessionRepository : ISessionRepository
    {
        private readonly HttpClient httpClient;
        private readonly UserRepository userRepository;

        public SessionRepository(HttpClient httpClient, UserRepository userRepository)
        {
            this.httpClient = httpClient;
            this.userRepository = userRepository;
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
            var reponse = await this.httpClient.PostAsJsonAsync($"api/session/{sessionKey}/join", user);

            return await reponse.Content.ReadAsAsync<UserStateResponseDTO>();
        }

        public async Task<RoundDTO> NextRoundAsync(string sessionKey)
        {
            var response = await this.httpClient.GetAsync($"api/session/{sessionKey}/items/rounds/next");

            return await response.Content.ReadAsAsync<RoundDTO>();
        }

        public async Task<RoundDTO> GetCurrentRound(string sessionKey)
        {
            var response = await this.httpClient.GetAsync($"api/session/{sessionKey}/items/rounds/current");

            return await response.Content.ReadAsAsync<RoundDTO>();
        }

        public async Task<ItemDTO> NextItemAsync(string sessionKey)
        {
            var response = await this.httpClient.GetAsync($"api/session/{sessionKey}/items/next");

            return await response.Content.ReadAsAsync<ItemDTO>();
        }

        public async Task<ICollection<ItemDTO>> GetAllItems(string sessionKey)
        {
            var response = await this.httpClient.GetAsync($"api/session/{sessionKey}/items");

            return await response.Content.ReadAsAsync<ICollection<ItemDTO>>();
        }

        public async Task<ItemDTO> GetCurrentItem(string sessionKey)
        {
            var response = await this.httpClient.GetAsync($"api/session/{sessionKey}/items/current");

            return await response.Content.ReadAsAsync<ItemDTO>();
        }

        public async Task<bool> Vote(string sessionKey, VoteDTO vote)
        {
            var response = await this.httpClient.PostAsJsonAsync($"api/session/{sessionKey}/items/rounds/votes", vote);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ThrowNitpickerCard(string sessionKey)
        {
            var response = await this.httpClient.PostAsync($"api/session/{sessionKey}/nitpicker", default (HttpContent));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> KickUser(string sessionKey, int userId)
        {
            var response = await this.httpClient.PostAsJsonAsync($"api/session/{sessionKey}", userId);

            return response.IsSuccessStatusCode;
        }
    }
}
