namespace PlanningPoker.Shared
{
    using System.ComponentModel.DataAnnotations;

    public class VoteCreateUpdateDTO
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int Estimate { get; set; }
    }
}
