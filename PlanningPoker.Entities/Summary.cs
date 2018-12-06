namespace PlanningPoker.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Summary : BaseEntity
    {
        [Required]
        public ICollection<ItemEstimate> ItemEstimates { get; set; }

        [Required]
        public int SessionId { get; set; }
    }
}
