using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracker_Server.Services.Authorization
{
    public class AuthService : IAuthService
    {
        // users not yet implemented
        public bool IsValidUser(string email, string password)
        {
            return true;
        }

        // users not yet implemented
        public Guid CreateSession(string email)
        {
            return Guid.Empty;
        }
    }
}
