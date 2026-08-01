using Application.Commands.CancelBooking;
using Application.Commands.CheckInBooking;
using Application.Commands.CheckOutBooking;
using Application.Commands.CreateBooking;
using Application.DTO;
using Application.Queries.GetAllBooking;
using Application.Queries.GetBookingById;
using Application.Queries.GetBookingsByUser;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
//using API.Models;

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

        [HttpGet("GetAllBooking")]
        [SwaggerOperation(Summary = "Lấy danh sách booking")]
        public async Task<ActionResult> GetAllBooking([FromQuery] int pageIndex = 1, int pageSize = 10)
        {
            var query = new GetAllBookingQuery
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetBookingById/{id}")]
        [SwaggerOperation(Summary = "Lấy booking theo id")]
        public async Task<ActionResult> GetBookingById([FromQuery] int id)
        {
            var query = new GetBookingByIdQuery
            {
                Id = id
            };
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("user/{userId}")]
        [SwaggerOperation(Summary = "Lấy danh sách booking theo user")]
        public async Task<ActionResult> GetBookingsByUser([FromQuery] int userId)
        {
            var query = new GetBookingsByUserQuery
            {
                UserId = userId
            };
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("CreateBooking")]
        [SwaggerOperation(Summary = "Thêm mới booking")]
        public async Task<ActionResult> CreateBooking([FromBody] CreateBookingDTO createBookingDTO)
        {
            var command = _mapper.Map<CreateBookingCommand>(createBookingDTO);
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{id}/cancel")]
        [SwaggerOperation(Summary = "Hủy booking")]
        public async Task<ActionResult> CancelBooking([FromRoute] int id)
        {
            var command = new CancelBookingCommand
            {
                BookingId = id
            };
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("CheckInBooking")]
        [SwaggerOperation(Summary = "Check in booking")]
        public async Task<ActionResult> CheckInBooking([FromBody] CheckInBookingDTO checkInBookingDTO)
        {
            var command = _mapper.Map<CheckInBookingCommand>(checkInBookingDTO);
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("CheckOutBooking")]
        [SwaggerOperation(Summary = "Check out booking")]
        public async Task<ActionResult> CheckOutBooking([FromBody] CheckOutBookingDTO checkOutBookingDTO)
        {
            var command = _mapper.Map<CheckOutBookingCommand>(checkOutBookingDTO);
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
    }
}