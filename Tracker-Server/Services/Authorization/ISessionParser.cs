using Microsoft.AspNetCore.Http;
using System;

namespace Tracker_Server.Services.Authorization
{
    interface ISessionParser
    {
        public Guid GetSessionID(HttpContext context);
    }
}
