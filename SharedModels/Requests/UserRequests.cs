﻿#pragma warning disable CS8618
namespace SharedModels.Requests
{
	public class UserLogin
	{
		public string UsernameOrEmail { get; set; }
		public string Password { get; set; }
	}
	public class UserRegister
	{
		public UserTypes Type { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string FirstName { get; set; } = "";
		public string LastName { get; set; } = "";
		public DateTime BirthDate { get; set; }
		public string Avatar { get; set; } = "";
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
	}

	public class UserPhoneLogin
	{
		public string PhoneNumber { get; set; }
	}

	public class ChangePassword
	{
		public string OldPassword { get; set; }
		public string NewPassword { get; set; }
	}
}
