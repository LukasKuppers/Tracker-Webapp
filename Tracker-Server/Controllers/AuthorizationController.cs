using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tracker_Server.Models.Authorization;
using Tracker_Server.Models.Users;
using Tracker_Server.Services.Authorization;
using Tracker_Server.Services.Constants;
using Tracker_Server.Services.DataAccess;
using Tracker_Server.Services.ActionFilters;

namespace Tracker_Server.Controllers
{
    [Route("api/authorization")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        [HttpGet]
        [AuthorizationFilter]
        public ActionResult<GetAuthOut> CheckAuth()
        {
            GetAuthOut response = new GetAuthOut()
            {
                Role = "user"
            };
            return Ok(response);
        }

        [HttpPut]
        public ActionResult<PutAuthOut> Login(PutAuthIn loginInfo)
        {
            if (loginInfo.Email == null || loginInfo.Email == "" ||
                loginInfo.Password == null || loginInfo.Password == "")
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
            if (regInfo.Email == null || regInfo.Email == "" ||
                regInfo.Password == null || regInfo.Password == "" ||
                regInfo.Username == null || regInfo.Username == "")
            {
                return BadRequest();
            }

            IResource resource = new Resource();
            IDbClient db = new DbClient(resource.GetString("db_base_path"));
            if (db.Contains<User, string>(resource.GetString("db_users_path"), "Email", regInfo.Email))
            {
                return Conflict();
            }

            // validate email and password
            try
            {
                new MailAddress(regInfo.Email);
            } catch
            {
                return UnprocessableEntity();
            }

            PasswordValidator pv = new PasswordValidator();
            if (!pv.IsValid(regInfo.Password))
            {
                return UnprocessableEntity();
            }


            IHashManager hashManager = new HashManager();
            Guid salt = Guid.NewGuid();
            string hash = hashManager.GetHash(salt.ToString(), regInfo.Password, 0);
            UserCredentials credentials = new UserCredentials()
            {
                PwdSalt = salt,
                PwdHash = hash
            };

            User newUser = new User()
            {
                Id = Guid.NewGuid(),
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
