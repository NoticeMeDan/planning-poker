using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlanningPoker.Entities
{
    public class Session
    {
        public int Id { get; set; }
        
        public string SessionKey { get; set; }

        [Required]
        public ICollection<Item> Items { get; set; }

        [Required]
        public ICollection<User> Users { get; set; }
    }
}
