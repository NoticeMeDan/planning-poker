namespace PlanningPoker.Shared
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class SessionCreateUpdateDTO
    {
        public int Id { get; set; }

        [StringLength(7)]
        public string SessionKey { get; set; }

        [Required]
        public ICollection<ItemCreateUpdateDTO> Items { get; set; }

        public ICollection<UserCreateDTO> Users { get; set; }
    }
}
