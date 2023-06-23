using Microsoft.EntityFrameworkCore;
using SharedModels;
using Users.API.Data;

namespace Users.API.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserServive _context;
        public UsersService(UserServive context)
        {
            _context = context;
        }

        public List<User> GetUsers()
        {
            return _context.Users.AsNoTracking().ToList();
        }
        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public User? GetUserById(Guid id)
        {
            return _context.Users.Find(id);
        }

        public User? GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public void UpdateUser(User user)
        {
            if (!_context.Users.Any(u => u.Id == user.Id))
                return;

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public List<Customer> GetCustomers()
        {
            return _context.Customers.AsNoTracking().ToList();
        }

        public Customer? GetCustomerByUserId(Guid userId)
        {
            return _context.Customers.FirstOrDefault(c => c.UserId == userId);
        }

        public void AddCustomer(Guid userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return;
            }

            user.Type = UserTypes.Customer;
            _context.Users.Update(user);

            var customer = new Customer
            {
                UserId = userId
            };

            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void UpdateCustomer(Customer customer)
        {
            var check = _context.Customers.FirstOrDefault(c => c.Id == customer.Id);
            if (check == null)
                return;

            _context.Customers.Update(customer);
            _context.SaveChanges();
        }
        public List<CustomerAddress> GetCustomerAddresses(Guid userId)
        {
            return _context.CustomerAddresses.AsNoTracking().Where(cA => cA.UserId == userId).ToList();
        }
        public void AddAddress(CustomerAddress address)
        {
            _context.CustomerAddresses.Add(address);
            _context.SaveChanges();
        }
        public void UpdateAddress(CustomerAddress address)
        {
            if (_context.CustomerAddresses.Any(a => a.Id == address.Id))
                return;

            _context.CustomerAddresses .Update(address);
            _context.SaveChanges();
        }
        public void DeleteAddress(CustomerAddress address)
        {
            if (_context.CustomerAddresses.Any(a => a.Id == address.Id))
                return;

            _context.CustomerAddresses.Remove(address);
            _context.SaveChanges();
        }
        public CustomerAddress? GetAddressById(Guid addressId)
        {
            return _context.CustomerAddresses.FirstOrDefault(s => s.UserId == addressId); ;
        }
        public List<Seller> GetSellers()
        {
            return _context.Sellers.AsNoTracking().ToList();
        }

        public Seller? GetSellerByUserId(Guid userId)
        {
            return _context.Sellers.FirstOrDefault(s => s.UserId == userId); ;
        }

        public void AddSeller(Guid userId)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception("User Not Found");
            }
            user.Type = UserTypes.Seller;
            _context.Users.Update(user);
            var seller = new Seller { UserId = userId };
            _context.Sellers.Add(seller);
            _context.SaveChanges();
        }

        public void UpdateSeller(Seller seller)
        {
            if (_context.Sellers.Any(s => s.Id == seller.Id))
                return;

            _context.Sellers.Update(seller);
            _context.SaveChanges();
        }
    }
}