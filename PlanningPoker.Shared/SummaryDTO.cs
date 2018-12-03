namespace PlanningPoker.Shared
{
    using System.Collections.Generic;
    using PlanningPoker.Entities;

    public class SummaryDTO
    {
        public int Id { get; set; }

        public ICollection<ItemEstimate> ItemEstimates { get; set; }

        public int SessionId { get; set; }
    }
}
