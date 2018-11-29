using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlanningPoker.Shared
{
    public class SessionCreateUpdateDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(7)]
        public string SessionKey { get; set; }

        [Required]
        public ICollection<ItemDTO> Items { get; set; }

        [Required]
        public ICollection<UserDTO> Users { get; set; }
    }
}
