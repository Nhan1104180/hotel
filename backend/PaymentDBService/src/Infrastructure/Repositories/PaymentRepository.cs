using System.Linq.Expressions;
using Application.Interfaces;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using PaymentDBService.src.Domain.Entities;
using PaymentDBService.src.Infrastructure.Data;

namespace Infrastructure.Repositories;

public class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
{
    private readonly PaymentDbContext _context;
    public PaymentRepository(PaymentDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByBookingIdAsync(int bookingId)
    {
        return await _context.Payments.AnyAsync(x => x.BookingId == bookingId && x.Status != PaymentStatus.Cancelled.ToString());
    }

    public async Task<Payment?> GetPaymenByIdAsync(int bookingId)
    {
        return await _context.Payments.FirstOrDefaultAsync(x => x.BookingId == bookingId);
    }

    public async Task<Payment> GetPaymentByIdAsync(int id)
    {
        return await _context.Payments.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<List<Payment>> WhereAsync(Expression<Func<Payment, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}