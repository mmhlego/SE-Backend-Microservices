using System.Security.Claims;
using System.Text;
using Chat.API.Models;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Mvc;
using SharedModels;
using SharedModels.Requests;
using Users.API.Services;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Users.API.Models.Requests;
using Microsoft.AspNetCore.Authorization;


namespace Users.API.Controllers
{
	[ApiController]
	[Route("api/[controller]/")]
	public class AuthController : ControllerBase
	{
		private readonly IUsersService _users;
		private readonly JwtTokenHandler _jwt;
		private readonly IVerificationService _verificationService;

		public AuthController(IUsersService users, JwtTokenHandler jwt, IVerificationService verificationService)
		{
			_users = users;
			_jwt = jwt;
			_verificationService = verificationService;
		}

		[HttpPost]
		[Route("login")]
		public ActionResult<AuthenticationResponse> Login([FromBody] UserLogin loginRequest)
		{
			if (!ModelState.IsValid)
			{
				return Ok(StatusResponse.Failed("خطایی رخ داده."));
			}
			User? user = _users.GetUserByUsername(loginRequest.UsernameOrEmail);
			if (user == null)
				user = _users.GetUserByEmail(loginRequest.UsernameOrEmail);

			if (user == null)
			{
				return Ok(StatusResponse.Failed("کاربر پیدا نشد."));

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
		[Route("loginByPhoneNumber")]
		public ActionResult<AuthenticationResponse> loginByPhoneNumber([FromBody] VerificationRequest v)
		{
			if (_verificationService.CheckVerificationCode(v.PhoneNumber, v.Code) == false)
				return Ok(StatusResponse.Failed("کد وارد شده صحیح نیست."));

			User? user = _users.GetUserByPhoneNumber(v.PhoneNumber);

			if (user == null)
			{
				return Ok(StatusResponse.Failed("کاربر پیدا نشد."));
			}
			else if (user.Restricted)
			{
				return Ok(StatusResponse.Failed("کاربر منع شده."));
			}

			return Ok(_jwt.GenerateJwtToken(user.Username, user.Id.ToString(), user.Type.ToString()));
		}

		[HttpPost]
		[Route("SendVerificationCode")]
		public ActionResult<StatusResponse> SendVerificationCode([FromBody] string phoneNumber)
		{
			if (_users.GetUserByPhoneNumber(phoneNumber) == null)
				return Ok(StatusResponse.Failed("خطایی رخ داده."));
			string code = GenerateVerificationCode();
			var accountSid = "AC6e20596d549049fcb7e7b447bf6f0cf4";
			var authToken = "c43260bfb6368ab4866294395c912e77";
			TwilioClient.Init(accountSid, authToken);

			var messageOptions = new CreateMessageOptions(new PhoneNumber(phoneNumber));

			messageOptions.From = new PhoneNumber("+18559554079");
			messageOptions.Body = ("Your code is " + code);

			var message = MessageResource.Create(messageOptions);
			Console.WriteLine(message.Body);
			bool res = _verificationService.SaveVerificationCode(phoneNumber, code);
			if (res)
				return Ok(StatusResponse.Success);
			else
				return Ok(StatusResponse.Failed("خطایی رخ داد."));
		}

		[HttpPost]
		[Route("register")]
		public ActionResult<AuthenticationResponse> RegisterUser([FromBody] UserRegister registerRequest)
		{
			if (!ModelState.IsValid)
			{
				return Ok(StatusResponse.Failed("خطایی رخ داده است."));
			}

			var username = _users.GetUserByUsername(registerRequest.Username);
			var email = _users.GetUserByEmail(registerRequest.Email);
			var phone = _users.GetUserByPhoneNumber(registerRequest.PhoneNumber);
			if (username != null || email != null || phone != null)
			{
				return Ok(StatusResponse.Failed("نام کاربری  یا ایمیل یا شماره تلفن از قبل وجود دارد."));
			}

			if (registerRequest.Type == UserTypes.Owner)
				return Ok(StatusResponse.Failed("اجازه ثبت نام ندارید."));
			if (registerRequest.Type == UserTypes.Admin || registerRequest.Type == UserTypes.StoreKeeper)
			{
				_ = Enum.TryParse(User.FindFirstValue(ClaimTypes.Role), out UserTypes role);
				if (role != UserTypes.Owner)
					return Ok(StatusResponse.Failed("اجازه ثبت نام ندارید."));
			}
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
		[Authorize]
		public ActionResult<StatusResponse> ChangePassword([FromBody] ChangePassword changePasswordRequest)
		{
			_ = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);
			User? user = _users.GetUserById(UserId);

			if (user == null)
				return Ok(StatusResponse.Failed("کاربر پیدا نشد."));

			if (user.Password != changePasswordRequest.OldPassword)
			{
				return Ok(StatusResponse.Failed("رمز نامعتبر است."));

			}

			user.Password = changePasswordRequest.NewPassword;
			_users.UpdateUser(user);

			return Ok(StatusResponse.Success);
		}

		private string GenerateVerificationCode()
		{
			const string validChars = "1234567890";
			StringBuilder code = new StringBuilder();

			Random random = new Random();

			for (int i = 0; i < 6; i++)
			{
				int index = random.Next(validChars.Length);
				code.Append(validChars[index]);
			}

			return code.ToString();
		}
	}
}