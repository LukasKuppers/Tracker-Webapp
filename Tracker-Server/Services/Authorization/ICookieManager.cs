using Microsoft.AspNetCore.Http;
using System;


namespace Tracker_Server.Services.Authorization
{
    public interface ICookieManager
    {
        public string GetCookie(HttpContext context, string key);

        public Guid GetSessionID(HttpContext context);
    }
}
