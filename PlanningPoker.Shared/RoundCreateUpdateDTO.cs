using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlanningPoker.Shared
{
    class RoundCreateUpdateDTO
    {
        public int Id { get; set; }

        [Required]
        public ICollection<VoteDTO> Votes { get; set; }
    }
}
