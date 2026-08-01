using System.Linq.Expressions;
using BookingDBService.Domain.Interfaces;
using BookingDBService.src.Domain.Entities;
using BookingDBService.src.Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BookingRepository : RepositoryBase<Booking>, IBookingRepository
{
    private readonly BookingDbContext _context;
    public BookingRepository(BookingDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Booking> AddAsync(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();
        return booking;
    }

    public async Task<List<Booking>> GetAllBookingAsync(int pageIndex, int pageSize)
    {
        return await _context.Bookings.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public Task<Booking?> GetBookingByIdAsync(int bookingId)
    {
        return _context.Bookings.FirstOrDefaultAsync(x => x.Id == bookingId);
    }

    public async Task<List<Booking>> GetBookingsByUserAsync(int userId)
    {
        return await _context.Bookings.Where(x => x.CustomerId == userId).ToListAsync();
    }

    public async Task<Booking?> GetByIdAsync(int id)
    {
        return await _context.Bookings.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<List<Booking>> WhereAsync(Expression<Func<Booking, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}