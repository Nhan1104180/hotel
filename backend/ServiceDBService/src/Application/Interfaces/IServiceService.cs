using Application.Commands.AddService;
using Application.Commands.RemoveService;
using Application.Commands.UpdateRoomStatus;
using Application.Commands.UpdateService;
using Application.Queries.GetServicesById;
using Application.Queries.SearchServices;
using Share.CommonModel;

namespace Application.Interfaces;

public interface IServiceService
{
    Task<ResponseEntity> GetAllServices();
    Task<ResponseEntity> GetServiceById(GetServicesByIdQuery query);
    Task<ResponseEntity> AddService(AddServiceCommand command);
    Task<ResponseEntity> UpdateService(UpdateServiceCommand command);
    Task<ResponseEntity> RemoveService(RemoveServiceCommand command);
    Task<ResponseEntity> UpdateServiceStatus(UpdateServiceStatusCommand command);
    Task<ResponseEntity> SearchService(SearchServicesQuery query);
}