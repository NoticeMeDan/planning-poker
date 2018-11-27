using System.ComponentModel.DataAnnotations;

namespace PlanningPoker.Entities
{
    public class ItemEstimate
    {
        public int Id { get; set; }

        [Required]
        public int Estimate { get; set; }

        [Required]
        public string ItemTitle { get; set; }
    }
}
