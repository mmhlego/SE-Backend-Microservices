using Microsoft.EntityFrameworkCore;
using SharedModels;
using Users.API.Data;

namespace Users.API.Services {
    public class UsersService : IUsersService {
        private readonly UsersContext _context;
        public UsersService(UsersContext context) {
            _context = context;
        }

        public IEnumerable<User> GetUsers() {
            return _context.Users.AsNoTracking();
        }

        public User GetUserById(Guid id) {
            if (id == Guid.Empty) {
                throw new ArgumentNullException("User id is empty");
            }

            var user = _context.Users.Find(id);
            if (user == null) {
                throw new ArgumentException("User not found");
            }

            return user;
        }

        public User GetUserByUsername(string username) {
            if (string.IsNullOrWhiteSpace(username)) {
                throw new ArgumentNullException("Username is empty");
            }

            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            if (user == null) {
                throw new ArgumentException("User not found");
            }

            return user;
        }

        public void UpdateUser(User user) {
            var u = _context.Users.SingleOrDefault(u => u.Id == user.Id);

            if (user == null) {
                throw new ArgumentNullException("User is null");
            }
            if (u == null) {
                throw new ArgumentNullException("User not found");
            }

            _context.Users.Update(u);
            _context.SaveChanges();
        }

        public IEnumerable<Customer> GetCustomers() {
            return _context.Customers.AsNoTracking();
        }

        public Customer GetCustomerByUserId(Guid userId) {
            if (userId == Guid.Empty) {
                throw new ArgumentNullException("User id is empty");
            }

            var customer = _context.Customers.SingleOrDefault(c => c.UserId == userId);
            if (customer == null) {
                throw new ArgumentException("Customer not found");
            }

            return customer;
        }

        public void AddCustomer(Guid userId) {
            if (userId == Guid.Empty) {
                throw new ArgumentNullException("User id is empty");
            }
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);

            if (user == null) {
                throw new Exception("User Not Found");
            }
            user.Type = UserTypes.Customer;
            _context.Users.Update(user);
            var customer = new Customer { UserId = userId };

            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void UpdateCustomer(Customer customer) {
            if (customer == null) {
                throw new ArgumentNullException("Customer is null");
            }
            var check = _context.Customers.SingleOrDefault(c => c.Id == customer.Id);
            if (check == null)
                throw new ArgumentException("Customer not found");
            check.Balance = customer.Balance;

            _context.Customers.Update(check);
            _context.SaveChanges();
        }

        public IEnumerable<Seller> GetSellers() {
            return _context.Sellers.ToList();
        }

        public Seller GetSellerByUserId(Guid userId) {
            if (userId == Guid.Empty) {
                throw new ArgumentNullException("User id is empty");
            }

            var seller = _context.Sellers.SingleOrDefault(s => s.UserId == userId);
            if (seller == null) {
                throw new Exception("Seller not found");
            }

            return seller;
        }

        public void AddSeller(Guid userId) {
            if (userId == Guid.Empty) {
                throw new ArgumentNullException("User id is empty");
            }
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null) {
                throw new Exception("User Not Found");
            }
            user.Type = UserTypes.Seller;
            _context.Users.Update(user);
            var seller = new Seller { UserId = userId };
            _context.Sellers.Add(seller);
            _context.SaveChanges();
        }

        public void UpdateSeller(Seller seller) {
            var s = _context.Sellers.SingleOrDefault(s => s.Id == seller.Id);

            if (seller == null) {
                throw new ArgumentNullException("Seller is null");
            }

            if (s == null) {
                throw new ArgumentNullException("Seller not found");
            }

            _context.Sellers.Update(seller);
            _context.SaveChanges();
        }
    }
}