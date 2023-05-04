using SharedModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Users.API.Models
{
    public class Seller
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Information { get; set; }
        public string Address { get; set; }

        [ForeignKey("UserId")]
        public User user
        {
            get; set;
        }
    }
}