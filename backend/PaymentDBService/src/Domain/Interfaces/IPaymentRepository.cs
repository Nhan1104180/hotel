using Domain.Interfaces.Base;
using PaymentDBService.src.Domain.Entities;

namespace Domain.Interfaces;

public interface IPaymentRepository : IRepositoryBase<Payment>
{
    Task<bool> ExistsByBookingIdAsync(int bookingId);
    Task<Payment> GetPaymentByIdAsync(int id);
    Task<Payment?> GetPaymenByIdAsync(int paymentId);
}