using SharedModels;

namespace Users.API.Services {
    public interface IUsersService {
        List<User> GetUsers();
        User? GetUserById(Guid id);
        User? GetUserByUsername(string username);
        void UpdateUser(User user);

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