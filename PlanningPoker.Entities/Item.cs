using System.Collections.Generic;

namespace PlanningPoker.Entities
{
    public class Item
    {
        public int Id { get; set; }

        public ICollection<Round> Rounds { get; set; }
    }
}
