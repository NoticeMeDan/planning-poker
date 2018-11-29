namespace PlanningPoker.Shared
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class RoundCreateUpdateDTO
    {
        public int Id { get; set; }

        [Required]
        public ICollection<VoteDTO> Votes { get; set; }
    }
}
