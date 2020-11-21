
using System.ComponentModel.DataAnnotations;

namespace Tracker_Web.Model.Login
{
    public class LoginForm
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
