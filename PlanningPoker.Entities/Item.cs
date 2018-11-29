using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PlanningPoker.Entities
{
    public class Item : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public ICollection<Round> Rounds { get; set; }

        public Item()
        {
            Rounds = new HashSet<Round>();
        }
    }
}
