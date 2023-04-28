﻿#pragma warning disable CS8618
namespace JwtAuthenticationManager.Models {
    public class AuthenticationResponse {
        public string UserName { get; set; }
        public string Role { get; set; }
        public string JwtToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
