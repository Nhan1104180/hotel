using Application.Commands.AddServiceUsage;
using Application.DTO;
using AutoMapper;
using ServiceDBService.src.Domain.Entities;

namespace Application.Mapping;

public class ServiceUsageProfile : Profile
{
    public ServiceUsageProfile()
    {
        CreateMap<ServiceUsage, ServiceUsageDTO>();
        CreateMap<AddServiceUsageDTO, AddServiceUsageCommand>();
    }
}