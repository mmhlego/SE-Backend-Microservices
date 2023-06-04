using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels;
using Users.API.Data;

namespace Users.API.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class SellerController : ControllerBase {
        private readonly UsersContext _context;
        public SellerController(UsersContext context)
        {
            _context = context;
        }
        [HttpPost]
        [Route("/test")]
        public void UpdateSeller(Seller seller)
        {
            if (seller == null)
            {
                throw new ArgumentNullException("Seller is null");
            }

            _context.Sellers.Update(seller);
            _context.SaveChanges();
        }
    }
}