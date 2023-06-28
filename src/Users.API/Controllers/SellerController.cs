using Chat.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels;
using SharedModels.Requests;
using System.Security.Claims;
using Users.API.Services;

namespace Users.API.Controllers
{
	[ApiController]
	[Route("api/[controller]/")]
	public class SellerController : ControllerBase
	{

		private readonly UsersService _seller;
		public SellerController(UsersService seller)
		{
			_seller = seller;
		}

		[HttpGet]
		[Route("sellers")]
		public ActionResult<List<Seller>> GetSellers(int page = 0, int perPage = 0)
		{
			List<Seller> sellers = _seller.GetSellers();
			Pagination<Seller> sellerPagination = Pagination<Seller>.Paginate(sellers, perPage, page);
			return Ok(sellerPagination);
		}

		[HttpGet]
		[Route("sellers/{id}")]
		public ActionResult<Seller> GetSeller(Guid userId)
		{
			Seller? seller = _seller.GetSellerByUserId(userId);
			if (seller == null)
			{
				return NotFound(StatusResponse.Failed("فروشنده موردنظر یافت نشد"));
			}

			return Ok(seller);
		}

		[HttpPut]
		[Route("sellers/{id}")]
		[Authorize(Roles = "Admin,Owner")]
		public ActionResult<Seller> UpdateSeller(Guid userId, [FromBody] UpdateSeller request)
		{
			Seller? seller = _seller.GetSellerByUserId(userId);
			if (seller == null)
			{
				return NotFound(StatusResponse.Failed("فروشنده  موردنظر یافت نشد"));
			}

			seller.Information = request.Information;
			seller.Address = request.Address;
			_seller.UpdateSeller(seller);
			return Ok(seller);
		}

		[HttpDelete]
		[Route("sellers/{id}")]
		[Authorize(Roles = "Admin,Owner")]
		public ActionResult<Seller> DeleteSeller(Guid userId)
		{
			User? seller = _seller.GetUserById(userId);
			if (seller == null)
			{
				return NotFound(StatusResponse.Failed("فروشنده  موردنظر یافت نشد"));
			}
			else
			{
				seller.Restricted = true;
				_seller.UpdateUser(seller);
				return Ok(seller);
			}
		}

		[HttpGet]
		[Route("profile")]
		[Authorize(Roles = "Seller")]
		public ActionResult<Seller> GetProfile(SellerProfile profile)
		{
			Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid currentUser);
			User user = _seller.GetUserById(currentUser)!;
			Seller seller = _seller.GetSellerByUserId(currentUser)!;

			profile.userInfo = user;
			profile.Information = seller.Information;
			profile.Address = seller.Address;
			return Ok(profile);
		}

		[HttpPut]
		[Route("profile")]
		[Authorize(Roles = "Seller")]
		public ActionResult<User> UpdateProfile([FromBody] UpdateSellerProfile profile)
		{
			Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid currentUser);
			Seller seller = _seller.GetSellerByUserId(currentUser)!;
			User user = _seller.GetUserById(currentUser)!;

			user.PhoneNumber = profile.PhoneNumber;
			user.LastName = profile.LastName;
			user.FirstName = profile.FirstName;
			user.Email = profile.Email;
			user.BirthDate = profile.BirthDate;
			user.Avatar = profile.Avatar;

			_seller.UpdateUser(user);

			seller.Address = profile.Address;
			seller.Information = profile.Information;

			_seller.UpdateSeller(seller);
			return Ok(profile);
		}
	}
}