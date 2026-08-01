using Application.Commands.AddRoom;
using Application.Commands.RemoveRoom;
using Application.Commands.UpdateRoom;
using Application.Commands.UpdateRoomStatus;
using Application.DTO;
using Application.Interfaces;
using Application.Queries.GetAvailableRooms;
using Application.Queries.GetRoomById;
using Application.Queries.SeachRoom;
using AutoMapper;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Share.CommonModel;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public RoomController(IRoomService roomService, IMediator mediator, IMapper mapper)
        {
            _roomService = roomService;
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("GetAllRooms")]
        [SwaggerOperation(Summary = "Lấy danh sách tất cả các phòng")]
        public async Task<ActionResult> GetAllRooms()
        {
            var result = await _roomService.GetAllRooms();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetRoomById/{id}")]
        [SwaggerOperation(Summary = "Lấy thông tin phòng theo ID")]
        public async Task<ActionResult> GetRoomById([FromRoute] int id)
        {
            var query = new GetRoomByIdQuery
            {
                Id = id
            };

            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("AddRoom")]
        [SwaggerOperation(Summary = "Thêm phòng")]
        public async Task<ActionResult> AddRoom([FromBody] AddRoomDTO room)
        {
            var command = _mapper.Map<AddRoomCommand>(room);

            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("UpdateRoom/{id}")]
        [SwaggerOperation(Summary = "Cập nhật phòng")]
        public async Task<ActionResult> UpdateRoom([FromRoute] int id, [FromBody] UpdateRoomDTO updateRoomDTO)
        {
            // Check id empty
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid user id"
                });
            }
            var command = _mapper.Map<UpdateRoomCommand>(updateRoomDTO);
            command.Id = id;

            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("DeleteRoom/{id}")]
        [SwaggerOperation(Summary = "Xóa phòng")]
        public async Task<ActionResult> DeleteRoom([FromRoute] int id)
        {
            var command = new RemoveRoomCommand
            {
                Id = id
            };

            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("SearchRoom")]
        [SwaggerOperation(Summary = "Tìm kiếm phòng")]
        public async Task<ActionResult> SearchRoom([FromQuery] string keyword, [FromQuery] int pageIndex = 1, int pageSize = 10)
        {
            var query = new SeachRoomQuery
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetRoomStatus")]
        [SwaggerOperation(Summary = "Xem trạng thái phòng")]
        public async Task<ActionResult> GetRoomStatus()
        {
            var result = Enum.GetValues(typeof(RoomStatus))
                .Cast<RoomStatus>()
                .Select(x => new
                {
                    Id = (int)x,
                    Name = x.ToString()
                });

            return Ok(new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "",
                Data = result
            });
        }

        [HttpGet("GetAvailableRooms")]
        [SwaggerOperation(Summary = "Kiểm tra phòng trống")]
        public async Task<ActionResult> GetAvailableRooms([FromRoute] int pageIndex = 1, int pageSize = 10)
        {
            var query = new GetAvailableRoomsQuery
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("{id}/status")]
        [SwaggerOperation(Summary = "Cập nhật trạng thái phòng")]
        public async Task<ActionResult> UpdateRoomStatus([FromRoute] int id, [FromBody] UpdateRoomStatusDTO updateRoomStatusDTO)
        {
            // Check id empty
            if (id < 0)
            {
                return BadRequest(new
                {
                    message = "Invalid user id"
                });
            }
            var command = _mapper.Map<UpdateRoomStatusCommand>(updateRoomStatusDTO);
            command.Id = id;

            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }


    }
}