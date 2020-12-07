using Microsoft.AspNetCore.Http;
using System;

namespace Tracker_Server.Services.Authorization
{
    public interface ISessionParser
    {
        public Guid GetSessionID(HttpContext context);
    }
}
