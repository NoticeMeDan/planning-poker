using System.ComponentModel.DataAnnotations;

namespace PlanningPoker.Entities
{
    public class Vote
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int Estimate { get; set; }
    }
}
