using System.ComponentModel.DataAnnotations;

namespace PlanningPoker.Entities
{
    public class ItemEstimate : BaseEntity
    {
        [Required]
        public int Estimate { get; set; }

        [Required]
        public string ItemTitle { get; set; }
    }
}
