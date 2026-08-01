using Application.Commands.CancelBooking;
using Application.Commands.CheckInBooking;
using Application.Commands.CheckOutBooking;
using Application.Commands.CreateBooking;
using Application.Queries.GetAllBooking;
using Application.Queries.GetBookingById;
using Application.Queries.GetBookingsByUser;
using Share.CommonModel;

namespace Application.Interfaces;

public interface IBookingService
{
    Task<ResponseEntity> GetAllBooking(GetAllBookingQuery query);
    Task<ResponseEntity> GetBookingById(GetBookingByIdQuery query);
    Task<ResponseEntity> GetBookingsByUser(GetBookingsByUserQuery query);
    Task<ResponseEntity> CreateBooking(CreateBookingCommand command);
    Task<ResponseEntity> CancelBooking(CancelBookingCommand command);
    Task<ResponseEntity> CheckInBooking(CheckInBookingCommand command);
    Task<ResponseEntity> CheckOutBooking(CheckOutBookingCommand command);
}