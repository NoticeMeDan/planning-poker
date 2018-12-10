namespace PlanningPoker.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Summary : BaseEntity
    {
        [Required]
        public List<ItemEstimate> ItemEstimates { get; set; }

        [Required]
        public int SessionId { get; set; }
    }
}
