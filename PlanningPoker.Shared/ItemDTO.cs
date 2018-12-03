using System.Collections.Generic;

namespace PlanningPoker.Shared
{
    public class ItemDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public ICollection<RoundDTO> Rounds { get; set; }
    }
}
