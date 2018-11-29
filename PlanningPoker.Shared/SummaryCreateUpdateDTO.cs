namespace PlanningPoker.Shared
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class SummaryCreateUpdateDTO
    {
        public int Id { get; set; }

        [Required]
        public ICollection<ItemEstimateDTO> ItemEstimates { get; set; }

        [Required]
        public int SessionId { get; set; }
    }
}
