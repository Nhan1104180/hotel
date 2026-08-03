using Application.Commands.AddService;
using Application.Commands.RemoveService;
using Application.Commands.UpdateRoomStatus;
using Application.Commands.UpdateService;
using Application.DTO;
using Application.Interfaces;
using Application.Queries.GetServicesById;
using Application.Queries.SearchServices;
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
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public ServiceController(IServiceService serviceService, IMediator mediator, IMapper mapper)
        {
            _serviceService = serviceService;
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("GetAllServices")]
        [SwaggerOperation(Summary = "Lấy danh sách tất cả các dịch vụ")]
        public async Task<ActionResult> GetAllServices()
        {
            var result = await _serviceService.GetAllServices();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetServiceById/{id}")]
        [SwaggerOperation(Summary = "Lấy thông tin dịch vụ theo ID")]
        public async Task<ActionResult> GetServiceById([FromRoute] int id)
        {
            var query = new GetServicesByIdQuery 
            {
                Id = id
            };
            
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("AddService")]
        [SwaggerOperation(Summary = "Thêm dịch vụ")]
        public async Task<ActionResult> AddService([FromBody] AddServiceDTO service)
        {
            var command = _mapper.Map<AddServiceCommand>(service);
            
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("UpdateService/{id}")]
        [SwaggerOperation(Summary = "Cập nhật dịch vụ")]
        public async Task<ActionResult> UpdateService([FromRoute] int id, [FromBody] UpdateServiceDTO updateServiceDTO)
        {
            var command = _mapper.Map<UpdateServiceCommand>(updateServiceDTO);
            command.Id = id;
            
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("RemoveService/{id}")]
        [SwaggerOperation(Summary = "Xóa dịch vụ")]
        public async Task<ActionResult> RemoveService([FromRoute] int id)
        {
            var command = new RemoveServiceCommand 
            {
                Id = id
            };
            
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("{id}/status")]
        [SwaggerOperation(Summary = "Cập nhật trạng thái dịch vụ")]
        public async Task<ActionResult> UpdateServiceStatus([FromRoute] int id, [FromBody] UpdateStatusDTO updateStatusDTO)
        {
            var command = _mapper.Map<UpdateServiceStatusCommand>(updateStatusDTO);
            command.Id = id;
            
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("SearchService")]
        [SwaggerOperation(Summary = "Tìm kiếm dịch vụ")]
        public async Task<ActionResult> SearchService([FromQuery] string keyword, [FromQuery] int pageIndex = 1, int pageSize = 10)
        {
            var query = new SearchServicesQuery 
            {
                Keyword = keyword,
                PageIndex = pageIndex, 
                PageSize = pageSize
            };
            
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetServiceStatus")]
        [SwaggerOperation(Summary = "Xem trạng thái dịch vụ")]
        public async Task<ActionResult> GetServiceStatus()
        {
            var result = Enum.GetValues(typeof(ServiceStatus))
                .Cast<ServiceStatus>()
                .Select(x => new
                {
                    Id = (int)x,
                    Name = x.ToString()
                });

            return Ok(new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Lấy danh sách trạng thái dịch vụ thành công",
                Data = result
            });
        }


    }
}