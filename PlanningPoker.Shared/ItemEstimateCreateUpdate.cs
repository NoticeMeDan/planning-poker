using System.ComponentModel.DataAnnotations;

namespace PlanningPoker.Shared
{
    public class ItemEstimateCreateUpdate
    {
        public int Id { get; set; }

        [Required]
        public int Estimate { get; set; }

        [Required]
        public string ItemTitle { get; set; }
    }
}
