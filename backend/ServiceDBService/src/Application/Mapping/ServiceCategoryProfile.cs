using Application.Commands.AddCategory;
using Application.Commands.UpdateCategory;
using Application.DTO;
using AutoMapper;

namespace Application.Mapping;

public class ServiceCategoryProfile : Profile
{
    public ServiceCategoryProfile()
    {
        CreateMap<AddCategoryDTO, AddCategoryCommand>();
        CreateMap<UpdateCategoryDTO, UpdateCategoryCommand>();
    }
}