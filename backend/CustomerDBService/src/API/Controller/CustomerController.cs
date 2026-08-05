using Application.Commands.AddCustomer;
using Application.Commands.RemoveCustomer;
using Application.Commands.UpdateCustomer;
using Application.DTO;
using Application.Queries.GetAllCustomer;
using Application.Queries.GetCustomerById;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public CustomerController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("GetAllCustomer")]
        [SwaggerOperation(Summary = "Lấy danh sách tất cả khách hàng" )]
        public async Task<ActionResult> GetAllCustomer([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllCustomerQuery
            {
                pageIndex = pageIndex,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetCustomerById/{id}")]
        [SwaggerOperation(Summary = "Lấy thông tin người dùng theo ID")]
        public async Task<ActionResult> GetCustomerById([FromRoute] int id)
        {
            var query = new GetCustomerByIdQuery
            {
                Id = id
            };

            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("AddCustomer")]
        [SwaggerOperation(Summary = "Thêm người dùng mới")]
        public async Task<ActionResult> AddCustomer([FromBody] CreateCustomerDTO createCustomerDTO)
        {
            var command = _mapper.Map<AddCustomerCommand>(createCustomerDTO);

            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("UpdateCustomer/{id}")]
        [SwaggerOperation(Summary = "Cập nhật thông tin người dùng")]
        public async Task<ActionResult> UpdateCustomer([FromRoute]int id, [FromBody] UpdateCustomerDTO updateCustomerDTO)
        {
            // Check id empty
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message ="Invalid user id"
                });
            }
            var command = _mapper.Map<UpdateCustomerCommand>(updateCustomerDTO);
            command.Id = id;

            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("RemoveCustomerId/{id}")]
        [SwaggerOperation(Summary = "Xóa người dùng theo ID")]
        public async Task<ActionResult> RemoveCustomerId([FromRoute] int id)
        {
            var command = new RemoveCustomerCommand
            {
                Id = id
            };

            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
    }
}