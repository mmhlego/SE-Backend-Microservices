using Chat.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedModels;
using SharedModels.Requests;
using System.Data;
using System.Security.Claims;
using Users.API.Services;

namespace Users.API.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class StoreKeeperController : ControllerBase
    {
        private readonly UsersService _storeKeeper;

        public StoreKeeperController(UsersService storeKeeper)
        {
            _storeKeeper = storeKeeper;
        }
        [HttpGet]
        [Route("storekeepers")]
        [Authorize(Roles = "Owner")]
        public ActionResult<List<User>> GetStoreKeepers(int page = 0, int perPage = 0)
        {
            List<User> storeKeepers = _storeKeeper.GetUsers().Where(u => u.Type == UserTypes.StoreKeeper).ToList();
            Pagination<User> storeKeeperPagination = Pagination<User>.Paginate(storeKeepers, perPage, page);
            return Ok(storeKeeperPagination);
        }

        [HttpGet]
        [Route("storekeepers/{id}")]
        [Authorize(Roles = "Owner")]
        public ActionResult<User> GetStoreKeeper(Guid userId)
        {
            User? storeKeeper = _storeKeeper.GetUserById(userId);
            if (storeKeeper == null)
            {
                return Ok(StatusResponse.Failed("انباردار  موردنظر یافت نشد"));
            }

            else
            {
                return Ok(storeKeeper);
            }
        }

        [HttpPut]
        [Route("storekeepers/{id}")]
        [Authorize(Roles = "Owner")]
        public ActionResult<User> UpdateStoreKeeper(Guid userId, [FromBody] UpdateAdmin request)
        {
            User? storeKeeper = _storeKeeper.GetUserById(userId);
            if (storeKeeper == null)
            {
                return Ok(StatusResponse.Failed("انباردار موردنظر یافت نشد"));
            }
            else
            {
                storeKeeper.FirstName = request.FirstName;
                storeKeeper.LastName = request.LastName;
                storeKeeper.PhoneNumber = request.PhoneNumber;
                storeKeeper.Email = request.Email;
                storeKeeper.BirthDate = request.BirthDate;
                _storeKeeper.UpdateUser(storeKeeper);
                return Ok(storeKeeper);
            }
        }


        [HttpDelete]
        [Route("StoreKeepers/{id}")]
        [Authorize(Roles = "Owner")]
        public ActionResult<User> DeleteAdmin(Guid userId)
        {
            User? StoreKeeper = _storeKeeper.GetUserById(userId);
            if (StoreKeeper == null)
            {
                return Ok(StatusResponse.Failed("ادمین  موردنظر یافت نشد"));
            }
            else
            {
                StoreKeeper.Restricted = true;
                _storeKeeper.UpdateUser(StoreKeeper);
                return Ok(StoreKeeper);
            }
        }

        [HttpGet]
        [Route("profile")]
        [Authorize(Roles = "StoreKeeper")]
        public ActionResult<User> GetProfile()
        {
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid currentUser);
            User? storeKeeper = _storeKeeper.GetUserById(currentUser);
            return Ok(storeKeeper);
        }

        [HttpPut]
        [Route("profile")]
        [Authorize(Roles = "StoreKeeper")]
        public ActionResult<User> UpdateProfile([FromBody] UpdateAdmin request)
        {
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid currentUser);
            User storeKeeper = _storeKeeper.GetUserById(currentUser);
            storeKeeper.FirstName = request.FirstName;
            storeKeeper.LastName = request.LastName;
            storeKeeper.PhoneNumber = request.PhoneNumber;
            storeKeeper.Email = request.Email;
            storeKeeper.BirthDate = request.BirthDate;
            _storeKeeper.UpdateUser(storeKeeper);
            return Ok(storeKeeper);
        }
    }
}
