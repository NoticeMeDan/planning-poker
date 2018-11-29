using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlanningPoker.Entities
{
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
            Items = new HashSet<Item>();
            Users = new HashSet<User>();
        }
    }
}
