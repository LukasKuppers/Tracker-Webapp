using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracker_Server.Services.Authorization
{
    interface IHashManager
    {
        // returns the Pbdkdf2 hash of Salt 'salt' combined with plain text pwd 'password',
        // with 'numIterations' iterations (should be > 1000) if numIterations = 0, use default (1000)
        public string GetHash(string salt, string password, int numIterations);
    }
}
