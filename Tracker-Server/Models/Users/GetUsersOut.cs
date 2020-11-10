using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracker_Server.Models.Users
{
    public class GetUsersOut
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public List<Guid> Projects { get; set; }
    }
}
