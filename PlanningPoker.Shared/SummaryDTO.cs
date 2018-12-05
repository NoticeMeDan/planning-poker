namespace PlanningPoker.Shared
{
    using System.Collections.Generic;

    public class SummaryDTO
    {
        public int Id { get; set; }

        public ICollection<ItemEstimateDTO> ItemEstimates { get; set; }

        public int SessionId { get; set; }
    }
}
