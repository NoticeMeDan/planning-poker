using System;
using System.Collections.Generic;
using System.Text;

namespace PlanningPoker.Shared
{
    public class RoundDTO
    {
        public int Id { get; set; }

        public ICollection<VoteDTO> Votes { get; set; }
    }
}
