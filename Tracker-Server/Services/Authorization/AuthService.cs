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

                // check if password is correct
                User user = users[0];
                IHashManager hashManager = new HashManager();

                string providedHash = hashManager.GetHash(user.Credentials.PwdSalt.ToString(), password, 0);
                if (providedHash == user.Credentials.PwdHash)
                {
                    return true; 
                }
            }
            return false;
        }

        // create a user session in the sessions table (a ID paired with a user ID)
        // for reference: users in this table are logged in, given they provide the correct sessionID
        public Guid CreateSession(string email)
        {
            // check if a user with the provided email exists
            var db = new DbClient("tracker");
            if (db.Contains<User, string>("users", "Email", email))
            {
                List<User> users = db.FindByField<User, string>("users", "Email", email);
                if (users.Count > 1)
                {
                    return Guid.Empty;   // we'll handle errors when we implement unit tests
                }

                User user = users[0];

                // check if the user already has an existing session
                if (db.Contains<Session, Guid>("sessions", "UserId", user.Id))
                {
                    Session session = db.FindByField<Session, Guid>("sessions", "UserId", user.Id)[0];
                    return session.Id;
                }

                // create a new user session
                Session newSession = new Session()
                {
                    Id = new Guid(),
                    UserId = user.Id
                };
                db.InsertRecord("sessions", newSession);
                return newSession.Id;
            }
            return Guid.Empty;
        }
    }
}
