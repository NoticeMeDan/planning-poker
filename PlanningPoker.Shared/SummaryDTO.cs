namespace PlanningPoker.Shared
{
    using System.Collections.Generic;

    public class SummaryDTO
    {
        public int Id { get; set; }

        public List<ItemEstimateDTO> ItemEstimates { get; set; }

        public int SessionId { get; set; }
    }
}
