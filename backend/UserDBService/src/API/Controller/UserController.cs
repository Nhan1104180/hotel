using Application.Commands.AddUser;
using Application.DTO;
using Application.Queries.GetAllUser;
using Application.Queries.GetUserById;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UserDBService.Application.Commands.RemoveUser;
using UserDBService.Application.Commands.UpdateUser;
//using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public UserController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("GetAllUser")]
        [SwaggerOperation(Summary = "Lấy danh sách tất cả người dùng" )]
        public async Task<ActionResult> GetAllUser([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllUserQuery
            {
                pageIndex = pageIndex,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetUserById/{id}")]
        [SwaggerOperation(Summary = "Lấy thông tin người dùng theo ID")]
        public async Task<ActionResult> GetUserById([FromRoute] int id)
        {
            var query = new GetUserByIdQuery
            {
                Id = id
            };

            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("AddUser")]
        [SwaggerOperation(Summary = "Thêm người dùng mới")]
        public async Task<ActionResult> AddUser([FromBody] UserCreateDTO userCreateDTO)
        {
            var command = _mapper.Map<AddUserCommand>(userCreateDTO);

            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("UpdateUser/{id}")]
        [SwaggerOperation(Summary = "Cập nhật thông tin người dùng")]
        public async Task<ActionResult> UpdateUser([FromRoute]int id, [FromBody] UserUpdateDTO userUpdateDTO)
        {
            // Check id empty
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message ="Invalid user id"
                });
            }
            var command = _mapper.Map<UpdateUserCommand>(userUpdateDTO);
            command.Id = id;

            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("RemoveUserId/{id}")]
        [SwaggerOperation(Summary = "Xóa người dùng theo ID")]
        public async Task<ActionResult> RemoveUserId([FromRoute] int id)
        {
            var command = new RemoveUserCommand
            {
                Id = id
            };

            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
    }
}