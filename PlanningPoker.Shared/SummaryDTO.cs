using System.Collections.Generic;

namespace PlanningPoker.Shared
{
    public class SummaryDTO
    {
        public int Id { get; set; }

        public ICollection<ItemEstimateDTO> Estimates { get; set; }

        public int SessionId { get; set; }
    }
}
