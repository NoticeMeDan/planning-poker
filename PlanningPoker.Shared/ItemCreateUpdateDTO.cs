using System.ComponentModel.DataAnnotations;

namespace PlanningPoker.Shared
{
    public class ItemCreateUpdateDTO
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        
        public string Description { get; set; }
    }
}
