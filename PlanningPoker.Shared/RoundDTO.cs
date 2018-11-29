using System.Collections.Generic;

namespace PlanningPoker.Shared
{
    public class RoundDTO
    {
        public int Id { get; set; }

        public ICollection<VoteDTO> Votes { get; set; }
    }
}
