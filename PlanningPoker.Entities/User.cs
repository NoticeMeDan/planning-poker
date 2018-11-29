using System.ComponentModel.DataAnnotations;

namespace PlanningPoker.Entities
{
    public class User : BaseEntity
    {
        [Required]
        public bool IsHost { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Nickname { get; set; }
    }
}
