namespace PlanningPoker.Shared
{
    using PlanningPoker.Entities;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class SummaryCreateUpdateDTO
    {
        public int Id { get; set; }

        [Required]
        public ICollection<ItemEstimate> ItemEstimates { get; set; }

        [Required]
        public int SessionId { get; set; }
    }
}
