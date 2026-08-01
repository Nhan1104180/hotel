using Application.Commands.AddRoomType;
using Application.Commands.RemoveRoomType;
using Application.Commands.UpdateRoomType;
using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypeController : ControllerBase
    {
        private readonly IRoomTypeService _roomTypeService;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public RoomTypeController(IRoomTypeService roomTypeService ,IMediator mediator, IMapper mapper)
        {
            _roomTypeService = roomTypeService;
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("GetAllRoomType")]
        [SwaggerOperation(Summary = "Lấy danh sách tất cả các loại phòng")]
        public async Task<ActionResult> GetAllRoomType()
        {
            var response = await _roomTypeService.GetAllRoomType();
            return StatusCode(response.StatusCode, response);
        }

         [HttpPost("AddRoomType")]
        [SwaggerOperation(Summary = "Thêm loại phòng")]
        public async Task<ActionResult> AddRoomType([FromBody] AddRoomTypeDTO roomType)
        {
            var command = _mapper.Map<AddRoomTypeCommand>(roomType);

            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("UpdateRoomType/{id}")]
        [SwaggerOperation(Summary = "Cập nhật loại phòng")]
        public async Task<ActionResult> UpdateRoomType([FromRoute] int id, [FromBody] UpdateRoomTypeDTO updateRoomTypeDTO)
        {
            var command = _mapper.Map<UpdateRoomTypeCommand>(updateRoomTypeDTO);
            command.Id = id;

            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("DeleteRoomType/{id}")]
        [SwaggerOperation(Summary = "Xóa loại phòng")]
        public async Task<IActionResult> DeleteRoomTypeById([FromRoute] int id)
        {
            var command = new RemoveRoomTypeCommand
            {
                Id = id
            };
            
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
    }
}