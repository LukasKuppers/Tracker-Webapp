using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tracker_Server.Models.Users;

namespace Tracker_Server.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public ActionResult<GetUsersOut> GetCurrent()
        {
            // get session ID cookie
            return Ok();
        }
    }
}
