namespace PlanningPoker.Shared
{
    using System.ComponentModel.DataAnnotations;

    public class UserCreateDTO
    {
        public int Id { get; set; }

        public bool IsHost { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Nickname { get; set; }
    }
}
