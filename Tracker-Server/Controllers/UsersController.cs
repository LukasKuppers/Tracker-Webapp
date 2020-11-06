using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tracker_Server.Models.Users;
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
        public ActionResult<GetUsersOut> GetCurrent()
        {
            CookieManager cookieManager = new CookieManager();
            Guid sessionID;
            try
            {
                sessionID = cookieManager.GetSessionID(HttpContext);
            } catch 
            {
                return BadRequest();
            }
            
            if (sessionID == Guid.Empty)
            {
                return BadRequest();
            }

            // check if there is a session corresponding to the sessionID
            Resource res = new Resource();
            IDbClient db = new DbClient(res.GetString("db_base_path"));

            List<Session> sessions = db.FindByField<Session, Guid>(res.GetString("db_sessions_path"), 
                "_id", sessionID);

            if (sessions.Count == 0)
            {
                return Unauthorized();
            }

            Session session = sessions[0];
            User user = db.FindByField<User, Guid>(res.GetString("db_users_path"),
                "_id", session.UserId)[0];

            GetUsersOut response = new GetUsersOut()
            {
                Username = user.Username,
                Email = user.Email,
                Projects = user.Projects
            };
            return Ok(response);
        }
    }
}
