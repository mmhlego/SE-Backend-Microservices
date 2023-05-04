using SharedModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Users.API.Models {
    public class Customer {

        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int Balance { get; set; }

        [ForeignKey("UserId")]
        public User user
        {
            get; set;
        }
    }
}