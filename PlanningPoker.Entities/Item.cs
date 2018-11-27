namespace PlanningPoker.Entities
{
    using System.Collections.Generic;

    public class Item : BaseEntity
    {
        public ICollection<Round> Rounds { get; set; }
    }
}
