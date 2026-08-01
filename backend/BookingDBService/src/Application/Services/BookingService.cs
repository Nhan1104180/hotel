using Application.Commands.CancelBooking;
using Application.Commands.CheckInBooking;
using Application.Commands.CheckOutBooking;
using Application.Commands.CreateBooking;
using Application.Interfaces;
using Application.Queries.GetAllBooking;
using Application.Queries.GetBookingById;
using Application.Queries.GetBookingsByUser;
using BookingDBService.Domain.Interfaces;
using BookingDBService.src.Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.ValueObjects;
using Infrastructure.Util;
using Share.CommonModel;

namespace Application.Services;

public class BookingService : IBookingService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly UnitOfWork _unitOfWork;
    public BookingService(IRoomRepository roomRepository, IBookingRepository bookingRepository, UnitOfWork unitOfWork)
    {
        _roomRepository = roomRepository;
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseEntity> GetAllBooking(GetAllBookingQuery query)
    {
        try
        {
            // Validate pageIndex and pageSize
            if (query.PageIndex < 0)
            {
                query.PageIndex = 1;
            }

            if (query.PageSize < 0)
            {
                query.PageSize = 10;
            }

            var bookings = await _bookingRepository.GetAllBookingAsync(query.PageIndex, query.PageSize);
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Lấy danh sách booking thành công",
                Data = bookings,
            };
        }
        catch (Exception ex)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
                Data = null,
            };
        }
    }

    public async Task<ResponseEntity> GetBookingById(GetBookingByIdQuery query)
    {
        try
        {
            if (query.Id <= 0)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Id không hợp lệ",
                    Data = null,
                };
            }

            var booking = await _bookingRepository.GetByIdAsync(query.Id);
            if (booking == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Không tìm thấy booking",
                    Data = null,
                };
            }
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Lấy danh sách booking thành công",
                Data = booking,
            };
        }
        catch (Exception ex)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
                Data = null,
            };
        }
    }
    
    public async Task<ResponseEntity> GetBookingsByUser(GetBookingsByUserQuery query)
    {
        try
        {
            if (query.UserId < 0)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Id không hợp lệ",
                    Data = null,
                };
            }

            var bookings = await _bookingRepository.GetBookingsByUserAsync(query.UserId);
            if (bookings == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Không tìm thấy booking",
                    Data = null,
                };
            }
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Lấy danh sách booking thành công",
                Data = bookings,
            };
        }
        catch (Exception ex)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
                Data = null,
            };
        }
    }

    public async Task<ResponseEntity> CreateBooking(CreateBookingCommand command)
    {
        try
        {
            //Kiểm tra phòng có tồn tại không
            var rooms = await _roomRepository.GetByIdAsync(command.RoomId);
            if (rooms == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Không tìm thấy phòng",
                    Data = null,
                };
            }

            var totalPrice = rooms.Price * (command.CheckOutDate - command.CheckInDate).Days;

            var booking = new Booking
            {
                CustomerId = command.CustomerId,
                RoomId = command.RoomId,
                CheckInDate = command.CheckInDate,
                CheckOutDate = command.CheckOutDate,
                TotalAmount = totalPrice,
                Status = BookingStatus.Pending.ToString()
            };

            // RoomServer set status = Occupied
            rooms.Status = RoomStatus.Occupied.ToString();
            await _roomRepository.UpdateAsync(rooms);

            //Thêm booking vào database
            await _bookingRepository.AddAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Đặt phòng thành công",
                Data = booking,
            };
        }
        catch (Exception ex)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
                Data = null,
            };
        }   
    }
    
    public async Task<ResponseEntity> CancelBooking(CancelBookingCommand command)
    {
        try
        {
            if (command.BookingId < 0)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Id không hợp lệ",
                    Data = null,
                };
            }

            var booking = await _bookingRepository.GetByIdAsync(command.BookingId);
            if (booking == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Không tìm thấy booking",
                    Data = null,
                };
            }

            //Status RoomService = Available
            var rooms = await _roomRepository.GetByIdAsync(booking.RoomId);
            if (rooms == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Không tìm thấy phòng",
                    Data = null,
                };
            }
            
            //Status BookingDBService = Cleaning
            rooms.Status = RoomStatus.Cleaning.ToString();
            await _roomRepository.UpdateAsync(rooms);

            //Status BookingDBService = Cancelled
            booking.Status = BookingStatus.Cancelled.ToString();
            await _bookingRepository.UpdateAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Hủy booking thành công",
                Data = booking,
            };
        }
        catch (Exception ex)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
                Data = null,
            };
        }
    }

    public async Task<ResponseEntity> CheckInBooking(CheckInBookingCommand command)
    {
        try
        {
            var booking = await _bookingRepository.GetByIdAsync(command.BookingId);
            if (booking == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Không tìm thấy booking",
                    Data = null,
                };
            }

           var CheckIn = new CheckInTime(command.CheckInDate);
           booking.CheckInDate = CheckIn.Value;
           await _bookingRepository.UpdateAsync(booking);
           await _unitOfWork.SaveChangesAsync();
    

            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Check-in thành công",
                Data = booking,
            };
        }
        catch (Exception ex)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
                Data = null,
            };
        }
    }

    public async Task<ResponseEntity> CheckOutBooking(CheckOutBookingCommand command)
    {
       try
       {
        var booking = await _bookingRepository.GetByIdAsync(command.BookingId);
        if (booking == null)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 404,
                Message = "Không tìm thấy booking",
                Data = null,
            };
        }

        var CheckOut = new CheckoutTime(command.CheckOutDate);
        booking.CheckOutDate = CheckOut.Value;
        await _bookingRepository.UpdateAsync(booking);
        await _unitOfWork.SaveChangesAsync();

        return new ResponseEntity
        {
            IsSuccess = true,
            StatusCode = 200,
            Message = "Check-out thành công",
            Data = booking,
        };
       }
       catch (Exception ex)
       {
           return new ResponseEntity
           {
               IsSuccess = false,
               StatusCode = 500,
               Message = ex.Message,
               Data = null,
           };
       }
    }
}
