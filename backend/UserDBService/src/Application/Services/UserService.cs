using Application.Commands.AddUser;
using Application.DTO;
using Application.Interfaces;
using Application.Queries.GetAllUser;
using Application.Queries.GetUserById;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using Infrastructure.Util;
using Share.CommonModel;
using UserDBService.Application.Commands.RemoveUser;
using UserDBService.Application.Commands.UpdateUser;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly UnitOfWork _unitOfWork;
    public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository, UnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseEntity> GetAllUsers(GetAllUserQuery query)
    {
        // Validate pageIndex
        if (query.pageIndex < 0)
        {
            query.pageIndex = 1;
        }
        if (query.PageSize <= 0)
        {
            query.PageSize = 10;
        }

        // Get all users
        var users = await _userRepository.GetAllUsers(query.pageIndex, query.PageSize);

        // Get total users
        var total = await _userRepository.CountAsync();

        //Empty response
        if (users == null || users.Any())
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 404,
                Message = "Không tìm thấy dữ liệu",
                Data = null,
            };
        }
        return new ResponseEntity
        {
            IsSuccess = true,
            StatusCode = 200,
            Message = "Lấy danh sách người dùng thành công",
            Data = users
        };
    }

    public async Task<ResponseEntity> GetUserById(GetUserByIdQuery query)
    {
        // Validate id
        if (query.Id <= 0)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 400,
                Message = "Invalid user ID",
                Data = null
            };
        }

        // Get user by id
        var user = await _userRepository.GetUserById(query.Id);

        //Empty response
        if (user == null)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 404,
                Message = "Không tìm thấy dữ liệu",
                Data = null,
            };
        }
        return new ResponseEntity
        {
            IsSuccess = true,
            StatusCode = 200,
            Message = "Lấy thông tin người dùng thành công",
            Data = user
        };
    }

    public async Task<ResponseEntity> CreateUser(AddUserCommand command)
    {
        try
        {
            //Check user exists
            var existingUser = await _userRepository.ExistsByUsernameAsync(command.Username);
            if (existingUser)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Username đã tồn tại",
                    Data = null,
                };
            }

            //Check email exists
            var existingEmail = await _userRepository.ExistsByEmailAsync(command.Email);
            if (existingEmail)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Email đã tồn tại",
                    Data = null,
                };
            }

            //Check phone exists
            var existingPhone = await _userRepository.ExistsByPhoneAsync(command.Phone);
            if (existingPhone)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Số điện thoại đã tồn tại",
                    Data = null,
                };
            }

            //Check role exists
            var Role = await _roleRepository.GetByNames(command.RoleNames);
            if (Role == null || !Role.Any())
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Role không tồn tại",
                    Data = null,
                };
            }

            // Validate ValueObjects
            var username = new UserName(command.Username);
            var fullName = new FullName(command.FullName);
            var email = new Email(command.Email);
            var password = new Password(command.Password);
            var phone = new Phone(command.Phone);
            var address = new Address(command.Address);
            // Validate command
            if (command == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Invalid command",
                    Data = null,
                };
            }

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

            //Save user
            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            command.Password = "********";

            //Add user role
            var userRoles = Role.Select(role => new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id,
                CreateAt = DateTime.Now
            }).ToList();

            await _userRoleRepository.AddRangeRole(userRoles);
            await _unitOfWork.SaveChangesAsync();

            // Return response
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Thêm người dùng thành công",
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
                Data = null,
            };
        }
    }

    public async Task<ResponseEntity> UpdateUser(UpdateUserCommand command)
    {
        // Find id
        var user = await _userRepository.GetUserById(command.Id);
        if (user == null)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 404,
                Message = "Id không tồn tại",
                Data = null,
            };
        }

        try
        {
            // Validate ValueObjects
            var username = new UserName(command.Username);
            var fullName = new FullName(command.FullName);
            var email = new Email(command.Email);
            var phone = new Phone(command.Phone);
            var address = new Address(command.Address);

            //Check user exists
            var existingUser = await _userRepository.ExistsByUsernameAsync(command.Username, command.Id);
            if (existingUser)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Username đã tồn tại",
                    Data = null,
                };
            }

            //Check email exists
            var existingEmail = await _userRepository.ExistsByEmailAsync(command.Email, command.Id);
            if (existingEmail)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Email đã tồn tại",
                    Data = null,
                };
            }

            //Check phone exists
            var existingPhone = await _userRepository.ExistsByPhoneAsync(command.Phone, command.Id);
            if (existingPhone)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Số điện thoại đã tồn tại",
                    Data = null,
                };
            }

            // Update entity
            user.Username = username.Value;
            user.FullName = fullName.Value;
            user.Email = email.Value;
            user.Phone = phone.Value;
            user.Address = address.Value;

            //Save user
            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            //Return response
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Cập nhật người dùng thành công",
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
                Data = null,
            };
        }
    }

    public async Task<ResponseEntity> DeleteUser(RemoveUserCommand command)
    {
        try
        {
            // Validate
            if (command.Id <= 0)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Invalid user ID"
                };
            }

            // Find user
            var user = await _userRepository.GetUserById(command.Id);
            if (user == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "User not found"
                };
            }

            // Get UserRoles
            var userRoles = await _userRoleRepository.GetByUserId(command.Id);
            if (userRoles.Any())
            {
                await _userRoleRepository.RemoveRange(userRoles);
                await _unitOfWork.SaveChangesAsync();
            }

            // Delete user
            await _userRepository.DeleteAsync(user);
            await _unitOfWork.SaveChangesAsync();

            // Return response
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Delete user successfully"
            };
        }
        catch (Exception ex)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
                Data = null,
            };
        }
    }
}