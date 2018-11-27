using System.Collections.Generic;

namespace PlanningPoker.Entities
{
    public class Summary : BaseEntity
    {
        public ICollection<ItemEstimate> ItemEstimates { get; set; }

        public int SessionId { get; set; }
    }
}
