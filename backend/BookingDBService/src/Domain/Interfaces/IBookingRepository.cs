using BookingDBService.src.Domain.Entities;
using Domain.Interfaces;

namespace BookingDBService.Domain.Interfaces;

public interface IBookingRepository :  IRepositoryBase<Booking>
{
    Task<List<Booking>> GetAllBookingAsync(int pageIndex, int pageSize);
    Task<List<Booking>> GetBookingsByUserAsync(int userId);
    Task<Booking?> GetBookingByIdAsync(int bookingId);
    Task<Booking> AddAsync(Booking booking);
    Task<Booking?> GetByIdAsync(int id);
}