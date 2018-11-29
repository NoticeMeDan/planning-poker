using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlanningPoker.Entities
{
    public class Round : BaseEntity
    {
        [Required]
        public ICollection<Vote> Votes { get; set; }

        public Round()
        {
            Votes = new HashSet<Vote>();
        }
    }
}
