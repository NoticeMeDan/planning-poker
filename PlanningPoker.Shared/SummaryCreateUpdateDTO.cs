using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlanningPoker.Shared
{    
    public class SummaryCreateUpdateDTO
    {
        public int Id { get; set; }

        [Required]
        public ICollection<ItemEstimateDTO> ItemEstimates { get; set; }

        [Required]
        public int SessionId { get; set; }
    }
}
