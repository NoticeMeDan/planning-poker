namespace PlanningPoker.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Vote : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int Estimate { get; set; }
    }
}
