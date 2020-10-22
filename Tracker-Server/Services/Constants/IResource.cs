using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracker_Server.Services.Constants
{
    public interface IResource
    {
        public string GetString(string key);
    }
}
