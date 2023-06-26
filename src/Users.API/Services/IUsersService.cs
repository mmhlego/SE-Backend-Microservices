using SharedModels;

namespace Users.API.Services {
    public interface IUsersService {
        List<User> GetUsers();
        User? GetUserById(Guid id);
        User? GetUserByUsername(string username);
        User? GetUserByEmail(string Email);
        User? GetUserByPhoneNumber(string phoneNumber);
        void UpdateUser(User user);
        void AddUser(User user);

        List<Customer> GetCustomers();
        Customer? GetCustomerByUserId(Guid userId);
        void AddCustomer(Guid userId);
        void UpdateCustomer(Customer customer);

        List<Seller> GetSellers();
        Seller? GetSellerByUserId(Guid userId);
        void AddSeller(Guid userId);
        void UpdateSeller(Seller seller);

        
    }
}