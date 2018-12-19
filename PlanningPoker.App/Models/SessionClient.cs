namespace PlanningPoker.App.Models
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Shared;

    public class SessionClient : ISessionClient
    {
        private readonly HttpClient httpClient;

        public SessionClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<SessionDTO> CreateAsync(SessionCreateUpdateDTO session)
        {
                var response = await this.httpClient.PostAsJsonAsync("api/session", session);

                var result = JsonConvert.DeserializeObject<SessionDTO>(response.Content.ReadAsStringAsync().Result);

                return result;
        }

        public async Task<bool> UpdateAsync(SessionCreateUpdateDTO session)
        {
            var response = await this.httpClient.PutAsJsonAsync($"api/session/{session.Id}", session);

            return response.IsSuccessStatusCode;
        }

        public async Task<SessionDTO> GetByKeyAsync(string sessionKey)
        {
            var response = await this.httpClient.GetAsync($"api/session/{sessionKey}");

            var result = JsonConvert.DeserializeObject<SessionDTO>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        public async Task<UserStateResponseDTO> Join(string sessionKey, UserCreateDTO user)
        {
            var response = await this.httpClient.PostAsJsonAsync($"api/session/{sessionKey}/join", user);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new KeyNotFoundException();
            }

            var result = JsonConvert.DeserializeObject<UserStateResponseDTO>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        public async Task<RoundDTO> NextRoundAsync(string sessionKey)
        {
            var response = await this.httpClient.PostAsJsonAsync($"api/session/{sessionKey}/item/round/next", string.Empty);

            var result = JsonConvert.DeserializeObject<RoundDTO>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        public async Task<RoundDTO> GetCurrentRound(string sessionKey)
        {
            var response = await this.httpClient.GetAsync($"api/session/{sessionKey}/item/round");

            var result = JsonConvert.DeserializeObject<RoundDTO>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        public async Task<ItemDTO> NextItemAsync(string sessionKey)
        {
            var response = await this.httpClient.PostAsJsonAsync($"api/session/{sessionKey}/item/next", string.Empty);

            var result = JsonConvert.DeserializeObject<ItemDTO>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        public async Task<ICollection<ItemDTO>> GetAllItems(string sessionKey)
        {
            var response = await this.httpClient.GetAsync($"api/session/{sessionKey}/item");

            var result = JsonConvert.DeserializeObject<ICollection<ItemDTO>>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        public async Task<ItemDTO> GetCurrentItem(string sessionKey)
        {
            var response = await this.httpClient.GetAsync($"api/session/{sessionKey}/item/current");

            var result = JsonConvert.DeserializeObject<ItemDTO>(response.Content.ReadAsStringAsync().Result);

            return result;
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

        public async Task<UserState> WhoAmI(string sessionKey)
        {
            var response = await this.httpClient.GetAsync($"api/session/{sessionKey}/whoami");

            var result = JsonConvert.DeserializeObject<UserState>(response.Content.ReadAsStringAsync().Result);

            return result;
        }
    }
}
