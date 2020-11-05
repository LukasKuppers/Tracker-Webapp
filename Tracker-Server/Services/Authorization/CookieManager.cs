using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracker_Server.Services.Constants;

namespace Tracker_Server.Services.Authorization
{
    public class CookieManager : ICookieManager
    {
        IResource resources;

        public CookieManager()
        {
            resources = new Resource();
        }

        public CookieManager(IResource resources)
        {
            this.resources = resources;
        }

        public string GetCookie(HttpContext context, string key)
        {
            if (context == null ||
                key == null || key == "")
            {
                throw new ArgumentNullException("Provided HTTP context or key is null or empty");
            }

            var cookies = context.Request.Cookies;
            string cookie = cookies[key];

            return cookie;
        }

        public Guid GetSessionID(HttpContext context)
        {
            string rawID = GetCookie(context, resources.GetString("cookies_session_key"));
            if (rawID == null) 
            {
                return Guid.Empty;
            }

            return Guid.Parse(rawID);
        }
    }
}
