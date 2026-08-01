using System.Data;
using Application.Commands.Login;
using Application.Commands.Logout;
using Application.Commands.Register;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.ValueObjects;
using IdentityDBService.Application.Commands.RefreshToken;
using IdentityDBService.src.Domain.Entities;
using Infrastructure.Util;
using Share.CommonModel;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly UnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;
    public AuthService(IUserRepository userRepository, UnitOfWork unitOfWork, IJwtService jwtService, IMapper mapper, IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _mapper = mapper;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<ResponseEntity> Register(RegisterCommand command)
    {
        try
        {
            // Kiểm tra xem email đã tồn tại chưa
            var existingEmail = await _userRepository.ExistsByEmailAsync(command.Email);
            if (existingEmail)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Email đã tồn tại",
                    Data = command
                };
            }
            //Kiểm tra xem username đã tồn tại chưa
            var existingUsername = await _userRepository.ExistsByUsernameAsync(command.Username);
            if (existingUsername)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Username đã tồn tại",
                    Data = command
                };
            }
            //Kiểm tra xem số điện thoại đã tồn tại chưa
            var existingPhoneNumber = await _userRepository.ExistsByPhoneNumberAsync(command.Phone);
            if (existingPhoneNumber)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Số điện thoại đã tồn tại",
                    Data = command
                };
            }

            //Tạo Username ValueObject
            var username = new UserName(command.Username);

            // Tạo Email ValueObject
            var email = new Email(command.Email);

            // Tạo Phone ValueObject
            var phone = new Phone(command.Phone);

            // Tạo Address ValueObject
            var address = new Address(command.Address);

            //Tạo FullName ValueObject
            var fullName = new FullName(command.FullName);

            //Tạo Password ValueObject
            var password = new Password(command.Password);

            //Map data từ dto sang model entity
            var user = new User
            {
                Username = username.Value,
                FullName = fullName.Value,
                Email = email.Value,
                Phone = phone.Value,
                CreateAt = DateTime.Now,
                PasswordHash = password.HashedValue,
                Address = address.Value
            };

            // Gọi repository để lưu người dùng
            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            command.Password = "******";

            // Default Role = User
            var roleID = new UserRole
            {
                UserId = user.Id,
                RoleId = 2, // User
            };

            await _userRepository.AddAsync(roleID);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Đăng ký thành công",
                Data = command
            };
        }
        catch (Exception ex)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
                Data = null
            };
        }
    }

    public async Task<ResponseEntity> Login(LoginCommand login)
    {
        try
        {
            // Create ValueObjects
            var identifier = new LoginIdentifier(login.EmailOrUsernameOrPhone);


            //Find user
            var user = await _userRepository.FindByIdentifierAsync(identifier.Value);

            // Verify password (nếu user null thì verify = false luôn, không throw)
            var verify = user != null && BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash);

            if (user == null || !verify)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "Tên đăng nhập/email/số điện thoại và mật khẩu không đúng",
                    Data = login
                };
            }

            // Generate token
            var accessToken = _jwtService.GenerateToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Lưu refresh token vào DB
            var refreshEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                CreateAt = DateTime.UtcNow
            };
            await _refreshTokenRepository.AddAsync(refreshEntity);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Đăng nhập thành công",
                Data = new
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                }
            };
        }
        catch (Exception ex)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
                Data = login
            };
        }
    }

    public async Task<ResponseEntity> RefreshToken(RefreshTokenCommand command)
    {
        try
        {
            // Validate
            var refreshTokenVo = new RefreshTokenValue(command.RefreshToken);

            // Find token
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshTokenVo.Value);

            // Not found
            if (refreshToken == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "Token không hợp lệ",
                    Data = command
                };
            }

            // Check revoked
            if (refreshToken.IsRevoked)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "Token đã bị thu hồi",
                    Data = command
                };
            }

            if (refreshToken.IsExpired())
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "Refresh token đã hết hạn"
                };
            }

            //Get user
            var user = await _userRepository.GetByIdAsync(refreshToken.UserId);

            // Generate new tokens
            var newAccessToken = _jwtService.GenerateToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            // Revoke old token
            refreshToken.Revoke();

            await _refreshTokenRepository.UpdateAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            // Save new refresh token
            var refreshEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                CreateAt = DateTime.UtcNow
            };
            await _refreshTokenRepository.AddAsync(refreshEntity);
            Console.WriteLine("Đã lưu RefreshToken");
            await _unitOfWork.SaveChangesAsync();

            // Return new tokens
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Refresh token thành công",
                Data = new
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                }
            };
        }
        catch (Exception ex)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
                Data = command
            };
        }
    }

    public async Task<ResponseEntity> Logout(LogoutCommand command)
    {
        try
        {
            // Validate
            var refreshTokenVo = new RefreshTokenValue(command.RefreshToken);

            // Find token
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshTokenVo.Value);

            // Token not found
            if (refreshToken == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "Token không hợp lệ",
                    Data = command
                };
            }

            // Already revoked
            if (refreshToken.IsRevoked)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "Token đã bị thu hồi",
                    Data = command
                };
            }

            // Revoke old token
            // refreshToken.Revoke();
            refreshToken.Revoke();
            await _refreshTokenRepository.UpdateAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            // Return new tokens
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Logout thành công",
                Data = new
                {
                    Message = "Refresh token revoked"
                }
            };
        }
        catch (Exception ex)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
                Data = command
            };
        }
    }
}