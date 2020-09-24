using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracker_Server.Models.Users;
using Tracker_Server.Services.DataAccess;

namespace Tracker_Server.Services.Authorization
{
    public class AuthService : IAuthService
    {
        public bool IsValidUser(string email, string password)
        {
            // check if a user with the given email exists
            var db = new DbClient("tracker");
            if (db.Contains<User, string>("users", "Email", email))
            {
                List<User> users = db.FindByField<User, string>("users", "Email", email);
                if (users.Count > 1)
                {
                    return false;   // we'll handle errors when we implement unit tests
                }

                User user = users[0];
                IHashManager hashManager = new HashManager();

                string providedHash = hashManager.GetHash(user.Credentials.PwdSalt.ToString(), password, 0);
                if (providedHash == user.Credentials.PwdHash)
                {
                    return true; 
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        // users not yet implemented
        public Guid CreateSession(string email)
        {
            return Guid.Empty;
        }
    }
}
