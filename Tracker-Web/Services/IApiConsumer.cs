using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Tracker_Web.Services
{
    public enum MethodType
    {
        GET,
        PUT,
        POST,
        DELETE
    }

    public interface IApiConsumer
    {
        public Task<(T, HttpStatusCode)> MakeEmptyRequest<T>(MethodType method, string path);

        public Task<(T, HttpStatusCode)> MakeRequest<T, U>(MethodType method, string path, U body);
    }
}
