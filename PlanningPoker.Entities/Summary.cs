using System.Collections.Generic;

namespace PlanningPoker.Entities
{
    public class Summary
    {
        public int Id { get; set; }

        public ICollection<ItemEstimate> ItemEstimates { get; set; }

        public int SessionId { get; set; }
    }
}
