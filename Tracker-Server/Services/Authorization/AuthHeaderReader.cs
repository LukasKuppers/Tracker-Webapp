using Microsoft.AspNetCore.Http;
using System;


namespace Tracker_Server.Services.Authorization
{
    public class AuthHeaderReader : ISessionParser
    {
        private static readonly string HEADER_KEY = "Authorization";

        public Guid GetSessionID(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException();
            }

            var headers = context.Request.Headers;
            
            if (headers.TryGetValue(HEADER_KEY, out var authVal))
            {
                Guid sessionID;
                try
                {
                    sessionID = Guid.Parse(authVal);
                } 
                catch 
                {
                    return Guid.Empty;
                }
                return sessionID;
            }
            return Guid.Empty;
        }
    }
}
