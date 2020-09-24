using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracker_Server.Services.Authorization
{
    public class HashManager : IHashManager
    {
        public string GetHash(string salt, string password, int numIterations)
        {
            byte[] hash = KeyDerivation.Pbkdf2(password, Encoding.ASCII.GetBytes(salt), KeyDerivationPrf.HMACSHA512, numIterations, 36);

            return Encoding.ASCII.GetString(hash);
        }
    }
}
