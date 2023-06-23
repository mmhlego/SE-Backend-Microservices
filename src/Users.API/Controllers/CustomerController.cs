using Azure.Core;
using Chat.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedModels;
using SharedModels.Requests;
using System.Security.Claims;
using Users.API.Services;

namespace Users.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class CustomerController : ControllerBase
    {
        private readonly UsersService _customers;

        public CustomerController(UsersService users)
        {
            _customers = users;
        }
        [HttpGet]
        [Route("customers")]
        [Authorize(Roles = "Admin,Owner")]
        public ActionResult<List<Customer>> GetCustomers(int page = 0, int perPage = 0)
        {
            List<Customer> customers = _customers.GetCustomers();
            Pagination<Customer> customerPagination = Pagination<Customer>.Paginate(customers, perPage, page);
            return Ok(customerPagination);
        }
        [HttpGet]
        [Route("customers/{id}")]
        [Authorize(Roles = "Admin,Owner")]
        public ActionResult<Customer> GetCustomer(Guid userId)
        {
            Customer? customer = _customers.GetCustomerByUserId(userId);
            if (customer == null)
            {
                return NotFound(StatusResponse.Failed("مشتری  موردنظر یافت نشد"));
            }

            else
            {
                return Ok(customer);
            }
        }

        [HttpPut]
        [Route("customers/{id}")]
        [Authorize(Roles = "Admin,Owner")]
        public ActionResult<Customer> UpdateCustomer(Guid userId, [FromBody] UpdateCustomer request)
        {
            Customer? customer = _customers.GetCustomerByUserId(userId);
            if (customer == null)
            {
                return NotFound(StatusResponse.Failed("مشتری  موردنظر یافت نشد"));
            }
            else
            {
                customer.Balance = request.Balance;
                _customers.UpdateCustomer(customer);
                return Ok(customer);
            }
        }

        [HttpDelete]
        [Route("customers/{id}")]
        [Authorize(Roles = "Admin,Owner")]
        public ActionResult<Customer> DeleteCustomer(Guid userId)
        {
            User? customer = _customers.GetUserById(userId);
            if (customer == null)
            {
                return NotFound(StatusResponse.Failed("مشتری  موردنظر یافت نشد"));
            }
            else
            {
                customer.Restricted = true;
                _customers.UpdateUser(customer);
                return Ok(customer);
            }
        }

        [HttpGet]
        [Route("profile")]
        [Authorize(Roles = "Customer")]
        public ActionResult<User> GetProfile()
        {
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid currentUser);
            User? customer = _customers.GetUserById(currentUser);
            return Ok(customer);
        }

        [HttpPut]
        [Route("profile")]
        [Authorize(Roles = "Customer")]
        public ActionResult<User> UpdateProfile([FromBody] UpdateAdmin request)
        {
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid currentUser);
            User customer = _customers.GetUserById(currentUser);
            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.PhoneNumber = request.PhoneNumber;
            customer.Email = request.Email;
            customer.BirthDate = request.BirthDate;
            _customers.UpdateUser(customer);
            return Ok(customer);
        }

        [HttpGet]
        [Route("addresses")]
        [Authorize(Roles = "Customer")]
        public ActionResult<CustomerAddress> GetAddresses()
        {
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid currentUser);
            List<CustomerAddress> addresses = _customers.GetCustomerAddresses(currentUser);
            return Ok(addresses);
        }

        [HttpPost]
        [Route("addresses")]
        [Authorize(Roles = "Customer")]
        public ActionResult<CustomerAddress> PostAddresse(PostAddress request) {

            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid currentUser);
            CustomerAddress customerAddress = new CustomerAddress
            {
                Id = Guid.NewGuid(),
                UserId = currentUser,
                Address = request.Address,
                City = request.City,
                ZipCode = request.ZipCode,
                Province = request.Province
            };
            _customers.AddAddress(customerAddress);
            return Ok(customerAddress);
        }

        [HttpGet]
        [Route("addresses/{id}")]
        [Authorize(Roles = "Customer")]
        public ActionResult<CustomerAddress> GetAddress(Guid addressId)
        {
            CustomerAddress? address = _customers.GetAddressById(addressId);
            if (address == null)
            {
                return NotFound(StatusResponse.Failed("آدرس  موردنظر یافت نشد"));
            }

            else
            {
                return Ok(address);
            }
        }


        [HttpPut]
        [Route("addresses/{id}")]
        [Authorize(Roles = "Customer")]
        public ActionResult<CustomerAddress> UpdateCustomerAddress(Guid addressId, [FromBody] UpdateAddress request)
        {
            CustomerAddress? address = _customers.GetAddressById(addressId);
            if (address == null)
            {
                return NotFound(StatusResponse.Failed("آدرس  موردنظر یافت نشد"));
            }
            else
            {
                address.Address = request.Address;
                address.Province = request.Province;
                address.City = request.City;
                address.ZipCode = request.ZipCode;
                _customers.UpdateAddress(address);
                return Ok(address);
            }
        }

        [HttpDelete]
        [Route("addresses/{id}")]
        [Authorize(Roles = "Customer")]
        public ActionResult<CustomerAddress> DeleteCustomerAddress(Guid addressId)
        {
            CustomerAddress? address = _customers.GetAddressById(addressId);
            if (address == null)
            {
                return NotFound(StatusResponse.Failed("آدرس  موردنظر یافت نشد"));
            }
            else
            {
                _customers.DeleteAddress(address);
                return Ok(address);
            }
        }

    }
}