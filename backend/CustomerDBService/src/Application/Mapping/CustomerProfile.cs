using Application.Commands.AddCustomer;
using Application.Commands.UpdateCustomer;
using Application.DTO;
using AutoMapper;

namespace Application.Mapping;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<CreateCustomerDTO, AddCustomerCommand>();
        CreateMap<UpdateCustomerDTO, UpdateCustomerCommand>();
    }
}
