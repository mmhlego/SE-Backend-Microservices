using Microsoft.AspNetCore.Mvc;
using Users.API.Services;
using SharedModels;
using SharedModels.Requests;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Users.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        UsersService _users;
        public AuthController(UsersService users)
        {
            _users = users;
        }
        [HttpPost]
        [Route("/login")]
        public IActionResult Login([FromBody] UserLogin loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User? user = _users.GetUserByUsername(loginRequest.Username);
            if (user == null)
            {
                return NotFound(new Dictionary<string, string>() { { "status", "Failed" } });
            }
            else if (user.Restricted)
            {
                return Ok(new Dictionary<string, string>() { { "status", "Restricted" } });
            }
            else
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Type.ToString())
                };

                //var token = new JwtSecurityToken(
                //                issuer: _configuration["Jwt:Issuer"],
                //                audience: _configuration["Jwt:Audience"],
                //                claims: claims,
                //                expires: DateTime.UtcNow.AddHours(2), // Set the token expiration as needed
                //                signingCredentials: crede
                //                 );

                //new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new Dictionary<string, string>() { { "status", "Success" } });
            }
        }

        [HttpPost]
        [Route("/register")]
        public IActionResult RegisterUser([FromBody] UserRegister registerRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                var status = new Dictionary<string, string>();
                var username = _users.GetUserByUsername(registerRequest.Username);

                if (username != null)
                {
                    ModelState.AddModelError("UserName", "This UserName Has been registered Already");
                    status = new Dictionary<string, string>() { { "status", "Exists" } };
                }
                else
                {
                    User user1 = new User()
                    {
                        Id = Guid.NewGuid(),
                        Type = registerRequest.Type,
                        Username = registerRequest.Username,
                        Password = registerRequest.Password,
                        FirstName = registerRequest.FirstName,
                        LastName = registerRequest.LastName,
                        PhoneNumber = registerRequest.PhoneNumber,
                        Email = registerRequest.Email,
                        BirthDate = registerRequest.BirthDate,
                        Avatar = "",
                        Verified = false,
                        Restricted = false
                    };
                    _users.AddUser(user1);

                    if (registerRequest.Type == UserTypes.Seller)
                    {
                        _users.AddSeller(user1.Id);
                    }
                    if (registerRequest.Type == UserTypes.Customer)
                    {
                        _users.AddCustomer(user1.Id);
                    }
                    if (user1 == null)
                    {
                        status = new Dictionary<string, string>() { { "status", "Failed" } };
                    }
                    status = new Dictionary<string, string>() { { "status", "Success" } };
                }
                return Ok(status);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("/changePassword")]
        public IActionResult ChangePassword([FromBody] ChangePassword changePasswordRequest) {

            User? user = _users.GetUserByUsername(changePasswordRequest.Username);
            if (user == null)
            {
                return NotFound(new Dictionary<string, string>() { { "status", "Failed" } });
            }
            else if (user.Password != changePasswordRequest.OldPassword)
            {
                return NotFound(new Dictionary<string, string>() { { "status", "Failed" } });
            }
            else
            {
                user.Password = changePasswordRequest.NewPassword;
                _users.UpdateUser(user);
            }
                return Ok();
        }
    }
}