using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracker_Server.Services.Authorization
{
    public interface IPasswordValidator
    {
        public bool IsValid(string password);
    }
}
