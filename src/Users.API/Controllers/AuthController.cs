using Chat.API.Models;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Mvc;
using SharedModels;
using SharedModels.Requests;
using Users.API.Services;

namespace Users.API.Controllers
{
	[ApiController]
	[Route("api/[controller]/")]
	public class AuthController : ControllerBase
	{
		private readonly IUsersService _users;
		private readonly JwtTokenHandler _jwt;

		public AuthController(IUsersService users, JwtTokenHandler jwt)
		{
			_users = users;
			_jwt = jwt;
		}

		[HttpPost]
		[Route("login")]
		public ActionResult<AuthenticationResponse> Login([FromBody] UserLogin loginRequest)
		{
			//TODO: UsernameOrEmail
			if (!ModelState.IsValid)
			{
				return BadRequest(StatusResponse.Failed("خطایی رخ داده."));
			}
			User? user = _users.GetUserByUsername(loginRequest.Username);
			if (user == null)
			{
				return NotFound(StatusResponse.Failed("کاربر پیدا نشد."));

			}
			else if (user.Restricted)
			{
				return Ok(StatusResponse.Failed("کاربر منع شده."));
			}

			Console.WriteLine(user.Id.ToString());
			Console.WriteLine(user.Type.ToString());
			Console.WriteLine(_jwt.GenerateJwtToken(user.Username, user.Id.ToString(), user.Type.ToString()).JwtToken);
			return Ok(_jwt.GenerateJwtToken(user.Username, user.Id.ToString(), user.Type.ToString()));
		}

		//TODO: POST - /loginByPhoneNumber

		[HttpPost]
		[Route("register")]
		public ActionResult<AuthenticationResponse> RegisterUser([FromBody] UserRegister registerRequest)
		{
			if (!ModelState.IsValid)
			{
				return StatusCode(StatusCodes.Status400BadRequest);
			}

			var username = _users.GetUserByUsername(registerRequest.Username);

			if (username != null)
			{
				return Ok(StatusResponse.Failed("نام کاربری از قبل وجود دارد."));
			}

			//TODO: Type==Customer / Seller => No Auth
			//TODO: Type==Admin / StoreKeeper => Owner
			//TODO: Type==Owner => Error

			User user = new User()
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

			_users.AddUser(user);

			if (registerRequest.Type == UserTypes.Seller)
			{
				_users.AddSeller(user.Id);
			}

			if (registerRequest.Type == UserTypes.Customer)
			{
				_users.AddCustomer(user.Id);
			}

			return Ok(_jwt.GenerateJwtToken(user.Username, user.Id.ToString(), user.Type.ToString()));
		}

		[HttpPut]
		[Route("changePassword")]
		//TODO: Authorize
		public ActionResult<StatusResponse> ChangePassword([FromBody] ChangePassword changePasswordRequest)
		{
			User? user = _users.GetUserByUsername(changePasswordRequest.Username);

			if (user == null)
				return NotFound(StatusResponse.Failed("کاربر پیدا نشد."));

			if (user.Password != changePasswordRequest.OldPassword)
			{
				return NotFound(StatusResponse.Failed("رمز نامعتبر است."));

			}

			user.Password = changePasswordRequest.NewPassword;
			_users.UpdateUser(user);

			return Ok(StatusResponse.Success);
		}
	}
}