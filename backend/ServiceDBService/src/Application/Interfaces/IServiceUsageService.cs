using Application.Commands.AddServiceUsage;
using Application.Commands.RemoveServiceUsage;
using Application.Queries.GetServiceUsageByBooking;
using Share.CommonModel;

namespace Application.Interfaces;

public interface IServiceUsageService
{
    Task<ResponseEntity> GetServiceUsageByBookingId(GetServiceUsageByBookingQuery query);
    Task<ResponseEntity> AddServiceUsage(AddServiceUsageCommand command);
    Task<ResponseEntity> DeleteServiceUsage(RemoveServiceUsageCommand command);
}