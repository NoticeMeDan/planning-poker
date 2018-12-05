namespace PlanningPoker.Shared
{
    using System.Collections.Generic;

    public class SessionDTO
    {
        public int Id { get; set; }

        public string SessionKey { get; set; }

        public ICollection<ItemDTO> Items { get; set; }

        public ICollection<UserDTO> Users { get; set; }
    }
}
