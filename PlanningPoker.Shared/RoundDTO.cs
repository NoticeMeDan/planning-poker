namespace PlanningPoker.Shared
{
    using System.Collections.Generic;

    public class RoundDTO
    {
        public int Id { get; set; }

        public ICollection<VoteDTO> Votes { get; set; }
    }
}
