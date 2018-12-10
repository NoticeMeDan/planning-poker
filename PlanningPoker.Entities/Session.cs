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
        public List<Item> Items { get; set; }

        [Required]
        public ICollection<User> Users { get; set; }

        public Session()
        {
            this.Items = new List<Item>();
            this.Users = new HashSet<User>();
        }
    }
}
