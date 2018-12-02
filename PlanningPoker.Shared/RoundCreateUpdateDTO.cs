namespace PlanningPoker.Shared
{
    using PlanningPoker.Entities;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class RoundCreateUpdateDTO
    {
        public int Id { get; set; }

        [Required]
        public ICollection<Vote> Votes { get; set; }
    }
}
