using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tracker_Server.Models.Authorization;
using Tracker_Server.Services.Authorization;

namespace Tracker_Server.Controllers
{
    [Route("api/authorization")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        [HttpPut]
        public ActionResult<PutAuthOut> Login(PutAuthIn loginInfo)
        {
            if(loginInfo == null ||
               loginInfo.Email == null ||
               loginInfo.Password == null)
            {
                return UnprocessableEntity();
            }

            if(loginInfo.Email == "" ||
               loginInfo.Password == "")
            {
                return BadRequest();
            }

            IAuthService authService = new AuthService();
            if(authService.IsValidUser(loginInfo.Email, loginInfo.Password))
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
    }
}
