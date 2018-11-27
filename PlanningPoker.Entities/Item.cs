using System.Collections.Generic;

namespace PlanningPoker.Entities
{
    public class Item : BaseEntity
    {
        public ICollection<Round> Rounds { get; set; }
    }
}
