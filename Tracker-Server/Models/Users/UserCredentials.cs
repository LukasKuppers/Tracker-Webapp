﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracker_Server.Models.Users
{
    public class UserCredentials
    {
        public Guid PwdSalt { get; set; }

        public string PwdHash { get; set; }
    }
}
