using System;
using System.Collections.Generic;
using SharedModels;

namespace Users.API.Services
{
    public interface IUsersService
    {
        IEnumerable<User> GetUsers();
        User GetUserById(Guid id);
        User GetUserByUsername(string username);
        void UpdateUser(User user);

        IEnumerable<Customer> GetCustomers();
        Customer GetCustomerByUserId(Guid userId);
        void AddCustomer(Guid userId);
        void UpdateCustomer(Customer customer);

        IEnumerable<Seller> GetSellers();
        Seller GetSellerByUserId(Guid userId);
        void AddSeller(Guid userId);
        void UpdateSeller(Seller seller);
    }
}