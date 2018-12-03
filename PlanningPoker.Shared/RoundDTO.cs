namespace PlanningPoker.Shared
{
    using PlanningPoker.Entities;
    using System.Collections.Generic;

    public class RoundDTO
    {
        public int Id { get; set; }

        public ICollection<Vote> Votes { get; set; }
    }
}
