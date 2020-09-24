using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracker_Server.Services.Authorization
{
    interface IAuthService
    {
        // check ifthe given password is valid for user (if extant) with email 'email'
        public bool IsValidUser(string email, string password);

        // create a new session for user with email 'email', return the session ID
        public Guid CreateSession(string email);
    }
}
