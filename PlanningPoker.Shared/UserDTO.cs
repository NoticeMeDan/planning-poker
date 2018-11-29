using System;
using System.Collections.Generic;
using System.Text;

namespace PlanningPoker.Shared
{
    public class UserDTO
    {
        public int Id { get; set; }

        public bool IsHost { get; set; }

        public string Email { get; set; }

        public string Nickname { get; set; }
    }
}
