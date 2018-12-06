namespace PlanningPoker.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Item : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public ICollection<Round> Rounds { get; set; }

        public Item()
        {
            this.Rounds = new HashSet<Round>();
        }
    }
}
