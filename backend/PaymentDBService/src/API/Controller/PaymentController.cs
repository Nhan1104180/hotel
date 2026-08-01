using Application.Commands.CreatePayment;
using Application.Commands.PaymentCallback;
using Application.DTO;
using Application.Queries.GetPaymentById;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
//using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public PaymentController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("CreatePayment")]
        [SwaggerOperation(Summary = "Tạo thanh toán")]
        public async Task<ActionResult> CreatePayment([FromBody] CreatePaymentDTO paymentDTO)
        {
            var command = _mapper.Map<CreatePaymentCommand>(paymentDTO);
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        } 

        [HttpGet("GetPaymentById")]
        [SwaggerOperation(Summary = "Lấy thanh toán theo ID")]
        public async Task<ActionResult> GetPaymentById([FromQuery] int id)
        {
            var query = new GetPaymentByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        } 

        [HttpPost("PaymentCallback")]
        [SwaggerOperation(Summary = "Callback xử lý kết quả thanh toán")]
        public async Task<ActionResult> PaymentCallback([FromBody] CreatePaymentDTO paymentDTO)
        {
            var command = _mapper.Map<PaymentCallbackCommand>(paymentDTO);
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }    
    }
}