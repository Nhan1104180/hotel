using Application.Commands.CreatePayment;
using Application.Commands.PaymentCallback;
using Application.Interfaces;
using Application.Queries.GetPaymentById;
using BookingDBService.Domain.Interfaces;
using Domain.Enums;
using Domain.Interfaces;
using Domain.ValueObjects;
using Infrastructure.Util;
using PaymentDBService.src.Domain.Entities;
using Share.CommonModel;

namespace Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly UnitOfWork _unitOfWork;

    public PaymentService(IPaymentRepository paymentRepository, IBookingRepository bookingRepository, UnitOfWork unitOfWork)
    {
        _paymentRepository = paymentRepository;
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseEntity> CreatePayment(CreatePaymentCommand command)
    {
        try
        {
            // 1. Kiểm tra Booking tồn tại
            var booking = await _bookingRepository.GetBookingByIdAsync(command.BookingId);
            if (booking == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Booking not found",
                    Data = null
                };
            }

            //2 .Không cho phép tạo Payment trùng
            if (await _paymentRepository.ExistsByBookingIdAsync(command.BookingId))
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Booking đã có giao dịch thanh toán.",
                    Data = null
                };
            }

            // 3. Value Objects
            var money = new Money(command.Amount);

            // 4. Tạo Payment 
            var payment = new Payment
            {
                BookingId = command.BookingId,
                Amount = money.Value,
                Status = PaymentStatus.Pending.ToString(),
                PaymentMethodId = command.PaymentMethodId,
                Description = command.Description,
                PaidAt = DateTime.UtcNow
            };

            // 5. Lưu vào DB
            await _paymentRepository.AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Payment created successfully",
                Data = payment
            };
        }

        catch (Exception ex)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
                Data = null
            };
        }
    }

    public async Task<ResponseEntity> GetPaymentById(GetPaymentByIdQuery query)
    {
        try
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(query.Id);
            if (payment == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Payment not found",
                    Data = null
                };
            }
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Payment found successfully",
                Data = payment
            };
        }
        catch (Exception ex)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
                Data = null
            };
        }
    }

    public async Task<ResponseEntity> PaymentCallback(PaymentCallbackCommand command)
    {
        try
        {
            var payment = await _paymentRepository.GetPaymenByIdAsync(command.BookingId);
            if (payment == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Không tìm thấy giao dịch thanh toán",
                    Data = null
                };
            }

            //Kiểm tra số tiền
            if (payment.Amount != command.Amount)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Số tiền thanh toán không khớp.",
                    Data = null
                };
            }

            var paymentInfo = new PaymentInformation(command.Amount, command.PaymentMethodId);
            
            // Business Logic
            payment.Complete(paymentInfo, command.Description);

            await _paymentRepository.UpdateAsync(payment);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Thanh toán thành công",
                Data = payment
            };
        }
        catch (Exception ex)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
                Data = null
            };
        }
    }
}