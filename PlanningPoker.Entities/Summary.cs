namespace PlanningPoker.Entities
{
    using System.Collections.Generic;

    public class Summary : BaseEntity
    {
        public ICollection<ItemEstimate> ItemEstimates { get; set; }

        public int SessionId { get; set; }
    }
}
