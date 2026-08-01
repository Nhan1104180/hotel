using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Commands.Login;
using Application.Commands.Logout;
using Application.Commands.Register;
using Application.DTO;
using AutoMapper;
using IdentityDBService.Application.Commands.RefreshToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
//using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public AuthController(IMediator mediator,IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("register")]
        //Ghi chú
        [SwaggerOperation(Summary = "Đăng ký tài khoản mới",Description = "Đăng ký tài khoản mới")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {   
            var command = _mapper.Map<RegisterCommand>(dto);

            var result = await _mediator.Send(command);

            return StatusCode(result.StatusCode, result);
        }
        
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Đăng nhập tài khoản",Description = "Đăng nhập bằng email, tên đăng nhập hoặc số điện thoại và mật khẩu để nhận token JWT và refresh token")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            // Thêm tạm dòng này để debug
            Console.WriteLine($"EmailOrUsernameOrPhone: {dto?.EmailOrUsernameOrPhone}, Password: {dto?.Password}");
            
            var command = _mapper.Map<LoginCommand>(dto);

            var result = await _mediator.Send(command);

            return StatusCode( result.StatusCode, result);
        }

        [HttpPost("refresh-token")]
        [SwaggerOperation(Summary = "Làm mới token",Description = "Làm mới token bằng refresh token để nhận token JWT và refresh token mới")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO dto)
        {
            var command = _mapper.Map<RefreshTokenCommand>(dto);

            var result = await _mediator.Send(command);

            return StatusCode( result.StatusCode, result);
        }

        [HttpPost("logout")]
        [SwaggerOperation(Summary = "Đăng xuất tài khoản",Description = "Đăng xuất tài khoản bằng refresh token để xóa token JWT và refresh token")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDTO dto)
        {
            var command = _mapper.Map<LogoutCommand>(dto);

            var result = await _mediator.Send(command);

            return StatusCode( result.StatusCode, result);
        }
    }
}