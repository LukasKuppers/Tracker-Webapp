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
                Id = currentUser.Id.ToString(), 
                Username = currentUser.Username,
                Email = currentUser.Email,
                Projects = currentUser.Projects
            };
            return Ok(response);
        }

        [HttpGet("{id}")]
        [AuthorizationFilter]
        public ActionResult<GetUsersOut> GetUser(string id)
        {
            if (id == "")
            {
                return BadRequest();
            }

            Guid userId = Guid.Parse(id);

            IResource resources = new Resource();
            IDbClient db = new DbClient(resources.GetString("db_base_path"));

            if (db.Contains<User, Guid>(resources.GetString("db_users_path"), "_id", userId))
            {
                User user = db.FindByField<User, Guid>(resources.GetString("db_users_path"), "_id", userId)[0];

                GetUsersOut response = new GetUsersOut()
                {
                    Id = user.Id.ToString(), 
                    Username = user.Username,
                    Email = user.Email,
                    Projects = user.Projects
                };

                return Ok(response);
            }

            return NotFound();
        }
        
    }
}
