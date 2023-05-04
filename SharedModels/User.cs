using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.API.Models;

namespace SharedModels
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public Guid Type { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImage { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Verified { get; set; }
        public bool Restricted { get; set; }
        [ForeignKey("Type")]
        public userType type { get; set; }
        public List<Seller> Sellers { get; set; }
        public List<Customer> Customers { get; set; }
    }
    public class userType
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }

    }
}
