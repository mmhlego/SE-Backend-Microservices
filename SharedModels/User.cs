using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618
namespace SharedModels {
    public class User {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public UserTypes Type { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Avatar { get; set; } = "";
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Verified { get; set; } = false;
        public bool Restricted { get; set; } = false;
    }
}
