namespace PlanningPoker.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Round : BaseEntity
    {
        [Required]
        public ICollection<Vote> Votes { get; set; }

        public Round()
        {
            this.Votes = new HashSet<Vote>();
        }
    }
}
