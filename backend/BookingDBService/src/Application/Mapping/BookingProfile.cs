using Application.Commands.CheckInBooking;
using Application.Commands.CheckOutBooking;
using Application.Commands.CreateBooking;
using Application.DTO;
using AutoMapper;
using BookingDBService.src.Domain.Entities;

namespace Application.Mapping;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<CreateBookingDTO, CreateBookingCommand>().ReverseMap(); //Create booking
        CreateMap<Booking, BookingDTO>().ReverseMap(); //Booking to DTO
        CreateMap<CheckInBookingDTO, CheckInBookingCommand>().ReverseMap(); //Check in booking
        CreateMap<CheckOutBookingDTO, CheckOutBookingCommand>().ReverseMap();  //Check out booking    
    }
}