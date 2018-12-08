namespace PlanningPoker.App.Models
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Shared;

    public interface IItemRepository
    {
        Task<ItemDTO> CreateAsync(ItemCreateUpdateDTO item);

        Task<ItemDTO> FindAsync(int itemId);

        Task<IEnumerable<ItemDTO>> ReadAsync();

        Task<bool> UpdateAsync(ItemCreateUpdateDTO item);

        Task<bool> DeleteAsync(int itemId);
    }
}
