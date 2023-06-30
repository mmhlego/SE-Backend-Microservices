using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Requests
{
	public class SellerProfile
	{
		public User userInfo { get; set; }
		public string Information { get; set; }
		public string Address { get; set; }
	}
	public class UpdateSeller
	{
		public string Information { get; set; }
		public string Address { get; set; }
	}
	public class SellerInfo //TODO UserId? No Password
	{
		public string Username { get; set; }
		public string Password { get; set; }
		public string FirstName { get; set; } = "";
		public string LastName { get; set; } = "";
		public DateTime BirthDate { get; set; }
		public string Avatar { get; set; } = "";
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string Information { get; set; }
		public string Address { get; set; }
	}
}
