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

		private readonly IUsersService _seller;
		public SellerController(IUsersService seller)
		{
			_seller = seller;
		}

		[HttpGet]
		[Route("sellers")]
		public ActionResult<Pagination<SellerInfo>> GetSellers(int page = 0, int perPage = 0, string searchText = "")
		{
			List<User> users = _seller.GetUsers().Where(u => u.Type == UserTypes.Seller).ToList();
			List<Seller> sellers = _seller.GetSellers();
			List<Seller> sellerInfoList = _seller.GetSellers();

			List<SellerInfo> sellersForShow = users
				.Join(sellerInfoList, user => user.Id, seller => seller.UserId, (user, seller) => sellerInfo(seller, user))
				.ToList();

			if (searchText != "")
			{
				sellersForShow = sellersForShow
			.Where(seller => seller.Username.Contains(searchText)
							 || seller.Address.Contains(searchText)
							 || seller.Information.Contains(searchText))
			.ToList();

			}
			Pagination<SellerInfo> sellerPagination = Pagination<SellerInfo>.Paginate(sellersForShow, perPage, page);
			return Ok(sellerPagination);
		}

		[HttpGet]
		[Route("sellers/{id}")]
		public ActionResult<SellerInfo> GetSeller(Guid userId)
		{
			User? user = _seller.GetUserById(userId);
			Seller? seller = _seller.GetSellerByUserId(userId);
			if (seller == null || user == null)
			{
				return Ok(StatusResponse.Failed("فروشنده موردنظر یافت نشد"));
			}
            SellerInfo sellerForShow = sellerInfo(seller, user);
            return Ok(sellerForShow);
		}

		[HttpPut]
		[Route("sellers/{id}")]
		[Authorize(Roles = "Admin,Owner")]
		public ActionResult<SellerInfo> UpdateSeller(Guid userId, [FromBody] SellerInfo profile)
		{
			Seller? seller = _seller.GetSellerByUserId(userId);
			if (seller == null)
			{
				return Ok(StatusResponse.Failed("فروشنده  موردنظر یافت نشد"));
			}

            _ = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid currentUser);
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

		[HttpDelete]
		[Route("sellers/{id}")]
		[Authorize(Roles = "Admin,Owner")]
		public ActionResult<SellerInfo> DeleteSeller(Guid userId)
		{

            User? user = _seller.GetUserById(userId);
            Seller? seller = _seller.GetSellerByUserId(userId);
            if (seller == null || user == null)
            {
                return Ok(StatusResponse.Failed("فروشنده موردنظر یافت نشد"));
            }

            user.Restricted = true;
			_seller.UpdateUser(user);

			return Ok(sellerInfo(seller, user));
		}

		[HttpGet]
		[Route("profile")]
		[Authorize(Roles = "Seller")]
		public ActionResult<SellerInfo> GetProfile()
		{
			Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid currentUser);
			User user = _seller.GetUserById(currentUser)!;
			Seller seller = _seller.GetSellerByUserId(currentUser)!;
			SellerInfo sellerForShow = sellerInfo(seller, user);
			return Ok(sellerForShow);
		}

		[HttpPut]
		[Route("profile")]
		[Authorize(Roles = "Seller")]
		public ActionResult<SellerInfo> UpdateProfile([FromBody] SellerInfo profile)
		{
			_ = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid currentUser);
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

		private SellerInfo sellerInfo(Seller seller, User user)
		{
			SellerInfo output = new SellerInfo
			{
				Username = user.Username,
				Password = user.Password,
				FirstName = user.FirstName,
				LastName = user.LastName,
				BirthDate = user.BirthDate,
				Avatar = user.Avatar,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				Address = seller.Address,
				Information = seller.Information
			};
			return output;
		}
	}
}