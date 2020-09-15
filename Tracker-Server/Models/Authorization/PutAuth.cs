using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracker_Server.Models.Authorization
{
    public class PutAuthOut
    {
        public string SessionId { get; set; }
    }

    public class PutAuthIn
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
