namespace PlanningPoker.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class ItemEstimate : BaseEntity
    {
        [Required]
        public int Estimate { get; set; }

        [Required]
        public string ItemTitle { get; set; }
    }
}
