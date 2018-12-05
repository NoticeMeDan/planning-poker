namespace PlanningPoker.Shared
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ItemCreateUpdateDTO
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public ICollection<RoundDTO> Rounds { get; set; }
    }
}
