using System;
using System.ComponentModel.DataAnnotations;

namespace Users.API.Models.Requests
{
    public class VerificationRequest
    {

        public string PhoneNumber { get; set; } = null!;
        public string Code { get; set; } = null!;
    }
}

