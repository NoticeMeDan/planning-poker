namespace PlanningPoker.Shared
{
    using PlanningPoker.Entities;
    using System.Collections.Generic;

    public class SessionDTO
    {
        public int Id { get; set; }

        public string SessionKey { get; set; }

        public ICollection<Item> Items { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
