using System.ComponentModel.DataAnnotations;

namespace PlanningPoker.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public bool IsHost { get; set; }

        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Nickname { get; set; }
    }
}
