using Chat.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using SharedModels;
using SharedModels.Requests;
using System.Data;
using System.Security.Claims;
using Users.API.Services;

namespace Users.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class AdminController : ControllerBase
    {

        UsersService _admin;
        public AdminController(UsersService admin)
        {
            _admin = admin;
        }
        [HttpGet]
        [Route("admins")]
        [Authorize(Roles = "Owner")]

        public ActionResult<List<User>> GetAdmins(int page = 0, int perPage = 0)
        {
            List<User> admins = _admin.GetUsers().Where(u => u.Type == UserTypes.Admin).ToList();
            Pagination<User> adminPagination = Pagination<User>.Paginate(admins, perPage, page);
            return Ok(adminPagination);
        }

        [HttpGet]
        [Route("admins/{id}")]
        [Authorize(Roles = "Owner")]
        public ActionResult<User> GetAdmin(Guid userId)
        {
            User? admin = _admin.GetUserById(userId);
            if (admin == null)
            {
                return NotFound(StatusResponse.Failed("ادمین  موردنظر یافت نشد"));
            }

            else
            {
                return Ok(admin);
            }
        }

        [HttpPut]
        [Route("admins/{id}")]
        [Authorize(Roles = "Owner")]
        public ActionResult<User> UpdateAdmin(Guid userId, [FromBody] UpdateAdmin request)
        {
            User? admin = _admin.GetUserById(userId);
            if (admin == null)
            {
                return NotFound(StatusResponse.Failed("ادمین موردنظر یافت نشد"));
            }
            else
            {
                admin.FirstName = request.FirstName;
                admin.LastName = request.LastName;
                admin.PhoneNumber = request.PhoneNumber;
                admin.Email = request.Email;
                admin.BirthDate = request.BirthDate;
                _admin.UpdateUser(admin);
                return Ok(admin);
            }
        }


        [HttpDelete]
        [Route("admins/{id}")]
        [Authorize(Roles = "Owner")]
        public ActionResult<User> DeleteAdmin(Guid userId)
        {
            User? admin = _admin.GetUserById(userId);
            if (admin == null)
            {
                return NotFound(StatusResponse.Failed("ادمین  موردنظر یافت نشد"));
            }
            else
            {
                admin.Restricted = true;
                _admin.UpdateUser(admin);
                return Ok(admin);
            }
        }

        [HttpGet]
        [Route("profile")]
        [Authorize(Roles = "Admin,Owner")]
        public ActionResult<User> GetProfile()
        {
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid currentUser);
            User admin = _admin.GetUserById(currentUser)!;
            return Ok(admin);
        }

        [HttpPut]
        [Route("profile")]
        [Authorize(Roles = "Admin,Owner")]
        public ActionResult<User> UpdateProfile([FromBody] UpdateAdmin request)
        {
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid currentUser);
            User admin = _admin.GetUserById(currentUser);
            admin.FirstName = request.FirstName;
            admin.LastName = request.LastName;
            admin.PhoneNumber = request.PhoneNumber;
            admin.Email = request.Email;
            admin.BirthDate = request.BirthDate;
            _admin.UpdateUser(admin);
            return Ok(admin);
        }
    }
}