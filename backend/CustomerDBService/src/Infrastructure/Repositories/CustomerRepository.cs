using Infrastructure.Data;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
{
    private readonly CustomerDbContext _context;
    public CustomerRepository(CustomerDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Customer>> GetAllCustomers(int pageNumber, int pageSize)
    {
        return await _context.Customers
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _context.Customers.CountAsync();
    }

    public async Task<Customer?> GetCustomerById(int id)
    {
        return await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Customers.AnyAsync(x => x.Email == email);
    }

    public async Task<bool> ExistsByPhoneAsync(string phone)
    {
        return await _context.Customers.AnyAsync(x => x.Phone == phone);
    }

    public async Task<Customer> CreateCustomer(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<bool> ExistsByEmailAsync(string email, int excludeId)
    {
        return await _context.Customers.AnyAsync(x => x.Email == email && x.Id != excludeId);
    }

    public async Task<bool> ExistsByPhoneAsync(string phone, int excludeId)
    {
        return await _context.Customers.AnyAsync(x => x.Phone == phone && x.Id != excludeId);
    }

    public Task<bool> ExistsByUsernameAsync(string username)
    {
        throw new NotImplementedException();
    }
}