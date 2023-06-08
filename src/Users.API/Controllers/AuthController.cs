using Chat.API.Models;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Mvc;
using SharedModels;
using SharedModels.Requests;
using Users.API.Services;

namespace Users.API.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase {
        private readonly UsersService _users;
        private readonly JwtTokenHandler _jwt;

        public AuthController(UsersService users, JwtTokenHandler jwt) {
            _users = users;
            _jwt = jwt;
        }

        [HttpPost]
        [Route("/login")]
        public ActionResult<AuthenticationResponse> Login([FromBody] UserLogin loginRequest) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            User? user = _users.GetUserByUsername(loginRequest.Username);
            if (user == null) {
                return NotFound(StatusResponse.Failed("User not found"));

            } else if (user.Restricted) {
                return Ok(StatusResponse.Failed("Restricted"));
            }

            return Ok(_jwt.GenerateJwtToken(user.Username, user.Id.ToString(), user.Type.ToString()));
        }

        [HttpPost]
        [Route("/register")]
        public ActionResult<AuthenticationResponse> RegisterUser([FromBody] UserRegister registerRequest) {
            if (!ModelState.IsValid) {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var username = _users.GetUserByUsername(registerRequest.Username);

            if (username != null) {
                return Ok(StatusResponse.Failed("Username already Exists."));
            }

            User user = new User() {
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

            _users.AddUser(user);

            if (registerRequest.Type == UserTypes.Seller) {
                _users.AddSeller(user.Id);
            }

            if (registerRequest.Type == UserTypes.Customer) {
                _users.AddCustomer(user.Id);
            }

            if (user == null) {
                return Ok(StatusResponse.Failed(""));
            }

            return Ok(_jwt.GenerateJwtToken(user.Username, user.Id.ToString(), user.Type.ToString()));
        }

        [HttpPut]
        [Route("/changePassword")]
        public ActionResult<StatusResponse> ChangePassword([FromBody] ChangePassword changePasswordRequest) {
            User? user = _users.GetUserByUsername(changePasswordRequest.Username);

            if (user == null)
                return NotFound(StatusResponse.Failed("User not found"));

            if (user.Password != changePasswordRequest.OldPassword) {
                return NotFound(StatusResponse.Failed("Invalid password"));

            }

            user.Password = changePasswordRequest.NewPassword;
            _users.UpdateUser(user);

            return Ok(StatusResponse.Success);
        }
    }
}