using AutoMapper;
using Application.DTO;
using Application.Commands.CreatePayment;
using Application.Commands.PaymentCallback;

namespace Application.Mapping;

public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        CreateMap<CreatePaymentDTO, CreatePaymentCommand>().ReverseMap();
        CreateMap<CreatePaymentDTO, PaymentCallbackCommand>().ReverseMap();
    }
}