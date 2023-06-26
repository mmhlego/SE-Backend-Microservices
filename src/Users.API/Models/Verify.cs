using System;
using System.ComponentModel.DataAnnotations;

namespace Users.API.Models
{
    public class Verify
    {
        [Key]
        public Guid Id { get; set; }
        [Phone]
        [Required]
        public string PhoneNumber { get; set; } = null!;
        public string Code { get; set; } = null!;
    }
}

