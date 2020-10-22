using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tracker_Server.Models.Authorization;
using Tracker_Server.Models.Users;
using Tracker_Server.Services.Authorization;
using Tracker_Server.Services.Constants;
using Tracker_Server.Services.DataAccess;

namespace Tracker_Server.Controllers
{
    [Route("api/authorization")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        [HttpPut]
        public ActionResult<PutAuthOut> Login(PutAuthIn loginInfo)
        {
            if (loginInfo == null ||
                loginInfo.Email == null ||
                loginInfo.Password == null)
            {
                return UnprocessableEntity();
            }

            if (loginInfo.Email == "" ||
                loginInfo.Password == "")
            {
                return BadRequest();
            }

            IResource resource = new Resource();
            IAuthService authService = new AuthService(new DbClient(resource.GetString("db_base_path")));
            if (authService.IsValidUser(loginInfo.Email, loginInfo.Password))
            {
                Guid sessID = authService.CreateSession(loginInfo.Email);
                PutAuthOut output = new PutAuthOut()
                {
                    SessionId = sessID.ToString()
                };
                return Ok(output);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        public ActionResult Register(PostRegIn regInfo)
        {
            if (regInfo == null ||
                regInfo.Email == null ||
                regInfo.Password == null ||
                regInfo.Username == null)
            {
                return UnprocessableEntity();
            }

            if (regInfo.Email == "" ||
                regInfo.Password == "" ||
                regInfo.Username == "")
            {
                return BadRequest();
            }

            // at some point we'll want to validate inputs (beyond not null)

            IResource resource = new Resource();
            IDbClient db = new DbClient(resource.GetString("db_base_path"));
            if (db.Contains<User, string>(resource.GetString("db_users_path"), "Email", regInfo.Email))
            {
                return Conflict();
            }

            IHashManager hashManager = new HashManager();
            Guid salt = new Guid();
            string hash = hashManager.GetHash(salt.ToString(), regInfo.Password, 0);
            UserCredentials credentials = new UserCredentials()
            {
                PwdSalt = salt,
                PwdHash = hash
            };

            User newUser = new User()
            {
                Id = new Guid(),
                Email = regInfo.Email, 
                Credentials = credentials, 
                Username = regInfo.Username,
                Projects = new List<Guid>()
            };
            db.InsertRecord(resource.GetString("db_users_path"), newUser);
            return CreatedAtAction("Register", regInfo);
        }
    }
}
