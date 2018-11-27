using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlanningPoker.Entities
{
    public class Round
    {
        public int Id { get; set; }

        [Required]
        public ICollection<Vote> Votes { get; set; }
    }
}
