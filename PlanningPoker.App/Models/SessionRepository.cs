namespace PlanningPoker.App.Models
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Shared;

    public class SessionRepository : ISessionRepository
    {
        private readonly HttpClient httpClient;
        private readonly string url = $"http://planningpoker-webapi.azurewebsites.net/";

        public SessionRepository(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<SessionDTO> CreateAsync(SessionCreateUpdateDTO session)
        {
            var response = await this.httpClient.PostAsJsonAsync($"{this.url}api/session", session);

            var result = JsonConvert.DeserializeObject<SessionDTO>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        public async Task<SessionDTO> FindAsync(int sessionId)
        {
            var response = await this.httpClient.GetAsync($"{this.url}api/session/{sessionId}");

            return await response.Content.ReadAsAsync<SessionDTO>();
        }

        public async Task<bool> UpdateAsync(SessionCreateUpdateDTO session)
        {
            var response = await this.httpClient.PutAsJsonAsync($"{this.url}api/session/{session.Id}", session);

            return response.IsSuccessStatusCode;
        }

        public async Task<SessionDTO> GetByKeyAsync(string sessionKey)
        {
            var response = await this.httpClient.GetAsync($"{this.url}api/session/{sessionKey}", HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);

            var result = JsonConvert.DeserializeObject<SessionDTO>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        public async Task<UserStateResponseDTO> Join(string sessionKey, UserCreateDTO user)
        {
            var response = await this.httpClient.PostAsJsonAsync($"{this.url}api/session/{sessionKey}/join", user);

            var result = JsonConvert.DeserializeObject<UserStateResponseDTO>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        public async Task<RoundDTO> NextRoundAsync(string sessionKey, string authToken)
        {
            var response = await this.httpClient.GetAsync($"{this.url}api/session/{sessionKey}/item/round/next", HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);

            var result = JsonConvert.DeserializeObject<RoundDTO>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        public async Task<RoundDTO> GetCurrentRound(string sessionKey)
        {
            var response = await this.httpClient.GetAsync($"{this.url}api/session/{sessionKey}/item/round");

            var result = JsonConvert.DeserializeObject<RoundDTO>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        public async Task<ItemDTO> NextItemAsync(string sessionKey)
        {
            var response = await this.httpClient.GetAsync($"{this.url}api/session/{sessionKey}/item/next", HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);

            var result = JsonConvert.DeserializeObject<ItemDTO>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        public async Task<ICollection<ItemDTO>> GetAllItems(string sessionKey)
        {
            var response = await this.httpClient.GetAsync($"{this.url}api/session/{sessionKey}/item");

            var result = JsonConvert.DeserializeObject<ICollection<ItemDTO>>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        public async Task<ItemDTO> GetCurrentItem(string sessionKey)
        {
            var response = await this.httpClient.GetAsync($"{this.url}api/session/{sessionKey}/item/current", HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);

            var result = JsonConvert.DeserializeObject<ItemDTO>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        public async Task<bool> Vote(string sessionKey, VoteDTO vote)
        {
            var response = await this.httpClient.PostAsJsonAsync($"{this.url}api/session/{sessionKey}/vote", vote);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ThrowNitpickerCard(string sessionKey)
        {
            var response = await this.httpClient.PostAsync($"{this.url}api/session/{sessionKey}/nitpicker", default(HttpContent));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> KickUser(string sessionKey, int userId)
        {
            var response = await this.httpClient.PostAsJsonAsync($"{this.url}api/session/{sessionKey}/user/kick", userId);

            return response.IsSuccessStatusCode;
        }
    }
}
