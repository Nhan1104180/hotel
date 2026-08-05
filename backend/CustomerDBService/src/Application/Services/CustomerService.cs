using Application.Commands.AddCustomer;
using Application.Commands.RemoveCustomer;
using Application.Commands.UpdateCustomer;
using Application.Interfaces;
using Application.Queries.GetAllCustomer;
using Application.Queries.GetCustomerById;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using Infrastructure.Util;
using Share.CommonModel;

namespace Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly UnitOfWork _unitOfWork;
    public CustomerService(ICustomerRepository customerRepository, UnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseEntity> GetAllCustomer(GetAllCustomerQuery query)
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
        var customers = await _customerRepository.GetAllCustomers(query.pageIndex, query.PageSize);

        // Get total users
        var total = await _customerRepository.CountAsync();    

        return new ResponseEntity
        {
            IsSuccess = true,
            StatusCode = 200,
            Message = "Lấy danh sách người dùng thành công",
            Data = customers
        };
    }

    public async Task<ResponseEntity> GetCustomerById(GetCustomerByIdQuery query)
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
        var customer = await _customerRepository.GetCustomerById(query.Id);

        //Empty response
        if (customer == null)
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
            Data = customer
        };
    }

    public async Task<ResponseEntity> CreateCustomer(AddCustomerCommand command)
    {
        try
        {
            //Check email exists
            var existingEmail = await _customerRepository.ExistsByEmailAsync(command.Email);
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
            var existingPhone = await _customerRepository.ExistsByPhoneAsync(command.Phone);
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

            // Validate ValueObjects
            var fullName = new FullName(command.FullName);
            var email = new Email(command.Email);
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
            var customer = new Customer
            {
                FullName = fullName.Value,
                Email = email.Value,
                Phone = phone.Value,
                Address = address.Value
            };

            //Save user
            await _customerRepository.AddAsync(customer);
            await _unitOfWork.SaveChangesAsync();

            // Return response
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Thêm khách hàng thành công",
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

    public async Task<ResponseEntity> UpdateCustomer(UpdateCustomerCommand command)
    {
        // Find id
        var user = await _customerRepository.GetCustomerById(command.Id);
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
            var fullName = new FullName(command.FullName);
            var email = new Email(command.Email);
            var phone = new Phone(command.Phone);
            var address = new Address(command.Address);

            //Check email exists
            var existingEmail = await _customerRepository.ExistsByEmailAsync(command.Email, command.Id);
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
            var existingPhone = await _customerRepository.ExistsByPhoneAsync(command.Phone, command.Id);
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
            user.FullName = fullName.Value;
            user.Email = email.Value;
            user.Phone = phone.Value;
            user.Address = address.Value;

            //Save user
            await _customerRepository.UpdateAsync(user);
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

    public async Task<ResponseEntity> DeleteCustomer(RemoveCustomerCommand command)
    {
        try
        {
            // Find user
            var user = await _customerRepository.GetCustomerById(command.Id);
            if (user == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "User not found"
                };
            }

            // Delete user
            await _customerRepository.DeleteAsync(user);
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