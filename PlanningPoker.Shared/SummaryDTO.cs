using System;
using System.Collections.Generic;
using System.Text;

namespace PlanningPoker.Shared
{
    class SummaryDTO
    {
        public int Id { get; set; }

        public ICollection<ItemEstimateDTO> Estimates { get; set; }

        public int SessionId { get; set; }
    }
}
