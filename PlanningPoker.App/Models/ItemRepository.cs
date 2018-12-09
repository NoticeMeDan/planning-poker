namespace PlanningPoker.App.Models
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Interfaces;
    using Shared;

    public class ItemRepository : IItemRepository
    {
        private readonly HttpClient httpClient;

        public ItemRepository(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ItemDTO> CreateAsync(ItemCreateUpdateDTO item)
        {
            var response = await this.httpClient.PostAsJsonAsync("api/items", item);

            return await response.Content.ReadAsAsync<ItemDTO>();
        }

        public async Task<ItemDTO> FindAsync(int itemId)
        {
            var response = await this.httpClient.GetAsync($"api/items/{itemId}");

            return await response.Content.ReadAsAsync<ItemDTO>();
        }

        public async Task<IEnumerable<ItemDTO>> ReadAsync()
        {
            var response = await this.httpClient.GetAsync("api/items");

            return await response.Content.ReadAsAsync<IEnumerable<ItemDTO>>();
        }

        public async Task<bool> UpdateAsync(ItemCreateUpdateDTO item)
        {
            var response = await this.httpClient.PutAsJsonAsync($"api/items/{item.Id}", item);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int itemId)
        {
            var response = await this.httpClient.DeleteAsync($"api/items/{itemId}");

            return response.IsSuccessStatusCode;
        }
    }
}
