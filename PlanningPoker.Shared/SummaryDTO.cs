namespace PlanningPoker.Shared
{
    using PlanningPoker.Entities;
    using System.Collections.Generic;

    public class SummaryDTO
    {
        public int Id { get; set; }

        public ICollection<ItemEstimate> Estimates { get; set; }

        public int SessionId { get; set; }
    }
}
