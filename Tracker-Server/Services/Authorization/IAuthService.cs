﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracker_Server.Services.Authorization
{
    interface IAuthService
    {
        public bool IsValidUser(string email, string password);

        public Guid CreateSession(string email);
    }
}
