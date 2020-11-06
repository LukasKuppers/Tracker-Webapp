using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracker_Server.Models.Users;
using Tracker_Server.Services.Authorization;
using Tracker_Server.Services.Constants;
using Tracker_Server.Services.DataAccess;

namespace Tracker_Server.Services.ActionFilters
{
    public class AuthorizationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // check if session cookie exists and is valid
            CookieManager cookieManager = new CookieManager();
            Guid sessionID;

            try
            {
                sessionID = cookieManager.GetSessionID(context.HttpContext);
            } catch
            {
                context.Result = new BadRequestResult();
                return;
            }
            
            if (sessionID == Guid.Empty)
            {
                context.Result = new BadRequestResult();
                return;
            }

            // check if session exists
            Resource res = new Resource();
            IDbClient db = new DbClient(res.GetString("db_base_path"));

            if (db.Contains<Session, Guid>(res.GetString("db_sessions_path"), "_id", sessionID))
            {
                Session session = db.FindByField<Session, Guid>(res.GetString("db_sessions_path"),
                    "_id", sessionID)[0];

                User user = db.FindByField<User, Guid>(res.GetString("db_users_path"),
                    "_id", session.UserId)[0];

                context.HttpContext.Items.Add("currentUser", user);
                return;
            }

            context.Result = new UnauthorizedResult();
        }
    }
}
