using System.ComponentModel.DataAnnotations;

namespace PlanningPoker.Entities
{
    public class Vote : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int Estimate { get; set; }
    }
}
