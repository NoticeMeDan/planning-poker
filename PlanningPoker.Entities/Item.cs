using System.ComponentModel.DataAnnotations;

namespace PlanningPoker.Entities
{
    using System.Collections.Generic;

    public class Item : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public ICollection<Round> Rounds { get; set; }
    }
}
