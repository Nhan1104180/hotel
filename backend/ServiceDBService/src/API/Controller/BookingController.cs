using Application.Commands.AddServiceUsage;
using Application.Commands.RemoveServiceUsage;
using Application.DTO;
using Application.Queries.GetServiceUsageByBooking;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public BookingController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("{bookingId}/services")]
        [SwaggerOperation(Summary = "Lấy danh sách dịch vụ của booking")]
        public async Task<ActionResult> GetServiceUsageByBooking(int bookingId)
        {
            var query = new GetServiceUsageByBookingQuery { BookingId = bookingId };
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{bookingId}/services")]
        [SwaggerOperation(Summary = "Thêm dịch vụ vào booking")]
        public async Task<ActionResult> AddServiceUsage(int bookingId, [FromBody] AddServiceUsageDTO request)
        { 
            var command = _mapper.Map<AddServiceUsageCommand>(request);
            command.BookingId = bookingId;

            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{bookingId}/services/{serviceId}")]
        [SwaggerOperation(Summary = "Xóa dịch vụ khỏi booking")]
        public async Task<ActionResult> RemoveServiceUsage(int bookingId, int serviceId)
        {
            var command = new RemoveServiceUsageCommand
            {
                BookingId = bookingId,
                ServiceId = serviceId
            };

            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
    }
}