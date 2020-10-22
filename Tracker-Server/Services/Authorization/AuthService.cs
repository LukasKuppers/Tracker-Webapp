using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracker_Server.Models.Users;
using Tracker_Server.Services.Constants;
using Tracker_Server.Services.DataAccess;

namespace Tracker_Server.Services.Authorization
{
    public class AuthService : IAuthService
    {
        private IDbClient db;
        private IResource resource;

        public AuthService(IDbClient dbClient)
        {
            db = dbClient;
            resource = new Resource();
        }

        public AuthService(IDbClient dbClient, IResource resource)
        {
            db = dbClient;
            this.resource = resource;
        }

        public bool IsValidUser(string email, string password)
        {
            // check if a user with the given email exists
            if (db.Contains<User, string>(resource.GetString("db_users_path"), "Email", email))
            {
                List<User> users = db.FindByField<User, string>(resource.GetString("db_users_path"), 
                    "Email", email);
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
            if (db.Contains<User, string>(resource.GetString("db_users_path"), "Email", email))
            {
                List<User> users = db.FindByField<User, string>(resource.GetString("db_users_path"), 
                    "Email", email);
                if (users.Count > 1)
                {
                    return Guid.Empty;   // we'll handle errors when we implement unit tests
                }

                User user = users[0];

                // check if the user already has an existing session
                if (db.Contains<Session, Guid>(resource.GetString("db_sessions_path"), 
                    "UserId", user.Id))
                {
                    Session session = db.FindByField<Session, Guid>(resource.GetString("db_sessions_path"), 
                        "UserId", user.Id)[0];
                    return session.Id;
                }

                // create a new user session
                Session newSession = new Session()
                {
                    Id = new Guid(),
                    UserId = user.Id
                };
                db.InsertRecord(resource.GetString("db_sessions_path"), newSession);
                return newSession.Id;
            }
            return Guid.Empty;
        }
    }
}
