using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Requests
{
    public class UserLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class UserRegister
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public UserTypes Type { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public DateTime BirthDate { get; set; }
        public string Avatar { get; set; } = "";
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class ChangePassword
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
