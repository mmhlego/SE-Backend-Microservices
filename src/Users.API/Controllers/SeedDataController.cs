using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels;
using System;
using System.Linq;
using Users.API.Data;

namespace Users.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedDataController : ControllerBase
    {
        private readonly UsersContext _context;
        private readonly Faker _faker;

        public SeedDataController(UsersContext context)
        {
            _context = context;
            _faker = new Faker("fa");
        }

        [HttpPost("Users")]
        public IActionResult SeedUsers(int count)
        {
            var userFaker = new Faker<User>("fa")
                .RuleFor(u => u.Id, f => Guid.NewGuid())
                .RuleFor(u => u.Type, f => f.PickRandom<UserTypes>())
                .RuleFor(u => u.Username, f => f.Person.UserName)
                .RuleFor(u => u.Password, f => f.Internet.Password())
                .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.BirthDate, f => f.Person.DateOfBirth)
                .RuleFor(u => u.Avatar, f => f.Person.Avatar)
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.PhoneNumber, f => f.Person.Phone)
                .RuleFor(u => u.Verified, f => f.Random.Bool())
                .RuleFor(u => u.Restricted, f => f.Random.Bool());

            var users = userFaker.Generate(count);
            _context.Users.AddRange(users);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("Sellers")]
        public IActionResult SeedSellers(int count)
        {
            var userIds = _context.Users.Where(u => u.Type == UserTypes.Seller).Select(u => u.Id).ToList();

            var sellerFaker = new Faker<Seller>("fa")
                .RuleFor(s => s.Id, f => Guid.NewGuid())
                .RuleFor(s => s.Information, f => f.Company.CompanyName())
                .RuleFor(s => s.Address, f => f.Address.FullAddress())
                .RuleFor(s => s.UserId, f => f.PickRandom(userIds));

            var sellers = sellerFaker.Generate(count);
            _context.Sellers.AddRange(sellers);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("Customers")]
        public IActionResult SeedCustomers(int count)
        {
            var userIds = _context.Users.Where(u => u.Type ==UserTypes.Customer).Select(u => u.Id).ToList();

            var customerFaker = new Faker<Customer>("fa")
                .RuleFor(c => c.Id, f => Guid.NewGuid())
                .RuleFor(c => c.Balance, f => f.Random.Long(0, 1000000))
                .RuleFor(c => c.UserId, f => f.PickRandom(userIds));

            var customers = customerFaker.Generate(count);
            _context.Customers.AddRange(customers);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("CustomerAddresses")]
        public IActionResult SeedCustomerAddresses(int count)
        {
            var userIds = _context.Users.Where(u => u.Type == UserTypes.Customer).Select(u => u.Id).ToList();
            var faker = new Faker("fa");
            var addresses = new List<CustomerAddress>();

            for (int i = 0; i < count; i++)
            {
                var address = new CustomerAddress
                { UserId = faker.PickRandom(userIds),
                    Province = faker.Address.State(),
                    City = faker.Address.City(),
                Address = faker.Address.StreetAddress(),
                    ZipCode = faker.Address.ZipCode()
                };

                addresses.Add(address);
            }

            _context.CustomerAddresses.AddRange(addresses);
            _context.SaveChanges();

            return Ok("Persian customer addresses seeded successfully.");
        }
    }
}
