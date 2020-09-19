using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tracker_Server.Models.Authorization;
using Tracker_Server.Models.Users;
using Tracker_Server.Services.Authorization;
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

            IAuthService authService = new AuthService();
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

        [Route("/register")]
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

            // at some point we'll want to add passowrd validation

            IDbClient db = new DbClient("tracker");
            if (db.Contains<User, string>("users", "Email", regInfo.Email))
            {
                return Conflict();
            }

            User newUser = new User()
            {
                Id = new Guid(),
                Credentials = new UserCredentials()
                {
                    Email = regInfo.Email,
                    PwdSalt = Guid.Empty,
                    PwdHash = ""
                },
                Username = regInfo.Username,
                Projects = new List<Guid>()
            };
            db.InsertRecord("users", newUser);
            return CreatedAtAction("Register", regInfo);
        }
    }
}
