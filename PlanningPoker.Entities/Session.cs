namespace PlanningPoker.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Session : BaseEntity
    {
        [Required]
        [StringLength(7)]
        public string SessionKey { get; set; }

        [Required]
        public ICollection<Item> Items { get; set; }

        [Required]
        public ICollection<User> Users { get; set; }

        public Session()
        {
            this.Items = new HashSet<Item>();
            this.Users = new HashSet<User>();
        }
    }
}
