namespace PlanningPoker.Shared
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using PlanningPoker.Entities;

    public class SessionCreateUpdateDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(7)]
        public string SessionKey { get; set; }

        [Required]
        public ICollection<Item> Items { get; set; }

        [Required]
        public ICollection<User> Users { get; set; }
    }
}
