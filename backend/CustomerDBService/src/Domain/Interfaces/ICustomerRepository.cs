using Domain.Entities;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface ICustomerRepository : IRepositoryBase<Customer>
{
    Task<List<Customer>> GetAllCustomers(int pageNumber, int pageSize);
    Task<int> CountAsync();
    Task<Customer?> GetCustomerById(int id);
    Task<bool> ExistsByEmailAsync(string email);
    Task<bool> ExistsByPhoneAsync(string phone);
    Task<Customer> CreateCustomer(Customer customer);
    Task<bool>ExistsByEmailAsync(string email,int excludeId);
    Task<bool>ExistsByPhoneAsync(string phone,int excludeId);
    
}