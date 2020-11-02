using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracker_Server.Services.Authorization
{
    public class PasswordValidator : IPasswordValidator
    {
        // enforce that passwords are at least 8 characters, and include an upper/lowercase letter, and a number
        public bool IsValid(String password)
        {
            if (password == null)
            {
                throw new ArgumentNullException();
            }

            if (password.Length >= 8)
            {
                if (password.Any(char.IsUpper) && 
                    password.Any(char.IsLower) &&
                    password.Any(char.IsDigit))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
