using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tracker_Server.Models.Users;
using Tracker_Server.Services.ActionFilters;
using Tracker_Server.Services.Authorization;
using Tracker_Server.Services.Constants;
using Tracker_Server.Services.DataAccess;

namespace Tracker_Server.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        [Route("current")]
        [AuthorizationFilter]
        public ActionResult<GetUsersOut> GetCurrent()
        {
            User currentUser = (User) HttpContext.Items["currentUser"];

            GetUsersOut response = new GetUsersOut()
            {
                Username = currentUser.Username,
                Email = currentUser.Email,
                Projects = currentUser.Projects
            };
            return Ok(response);
        }
    }
}
