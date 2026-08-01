using Application.Commands.AddService;
using Application.Commands.RemoveService;
using Application.Commands.UpdateRoomStatus;
using Application.Commands.UpdateService;
using Application.Interfaces;
using Application.Queries.GetServicesById;
using Application.Queries.SearchServices;
using Domain.Enums;
using Domain.Interfaces;
using Domain.ValueObjects;
using Infrastructure.Util;
using ServiceDBService.src.Domain.Entities;
using Share.CommonModel;

namespace Application.Services;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IServiceCategoryRepository _categoryRepository;
    private readonly UnitOfWork _unitOfWork;
    public ServiceService(IServiceRepository serviceRepository, IServiceCategoryRepository categoryRepository, UnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseEntity> GetAllServices()
    {
        try
        {
            var services = await _serviceRepository.GetAllServicesAsync();
            if (services == null || !services.Any())
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Không tìm thấy dịch vụ",
                    Data = null
                };
            }
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Lấy danh sách dịch vụ thành công",
                Data = services
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

    public async Task<ResponseEntity> GetServiceById(GetServicesByIdQuery query)
    {
        //Get room by Id
        var service = await _serviceRepository.GetServiceByIdAsync(query.Id);

        //Empty response
        if (service == null)
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
            Data = service
        };
    }

    public async Task<ResponseEntity> AddService(AddServiceCommand command)
    {
        try
        {
            // Check exists
            var exists = await _serviceRepository.ExistsByNameAsync(command.Name);

            if(exists)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Tên dịch vụ đã tồn tại",
                    Data = null
                };
            }

            //Kiểm tra CategoryId có tồn tại không ExistsByIdAsync
            var existsCategory = await _categoryRepository.GetByIdAsync(command.CategoryId);
            if(existsCategory == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "CategoryId không tồn tại",
                    Data = null
                };
            }

            // ValueObjects
            var name = new ServiceName(command.Name);
            var money = new Money(command.Price);
            var description = new Description(command.Description);

            // Create room
            var service = new Service
            {
                CategoryId = command.CategoryId,
                Name = name.Value,
                Description = description.Value,
                Price = money.Value,
                Status = ServiceStatus.Active.ToString(),
                CreatedAt = DateTime.Now,
                ImageUrl = command.ImageUrl
            };

            // Add room
            await _serviceRepository.AddServiceAsync(service);
            await _unitOfWork.SaveChangesAsync();
            
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Thêm dịch vụ thành công",
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

    public async Task<ResponseEntity> UpdateService(UpdateServiceCommand command)
    {
        try
        {
            // Check service exists
            var service = await _serviceRepository.GetByIdAsync(command.Id);
            if(service == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Id không tồn tại",
                    Data = null
                };
            }

            // Check CategoryId exists
            var category = await _categoryRepository.GetByIdAsync(command.CategoryId);
            if(category == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Loại dịch vụ không tồn tại",
                    Data = null
                };
            }

            // ValueObjects
            var name = new ServiceName(command.Name);
            var money = new Money(command.Price);
            var description = new Description(command.Description);

            // Update service
            service.CategoryId = command.CategoryId;
            service.Name = name.Value;
            service.Description = description.Value;
            service.Price = money.Value;
            if(!string.IsNullOrEmpty(command.ImageUrl))
            {
                service.ImageUrl = command.ImageUrl;
            }
        
            // Update service
            await _serviceRepository.UpdateAsync(service);
            await _unitOfWork.SaveChangesAsync();
            
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Cập nhật dịch vụ thành công",
                Data = command
            };
        }
        catch (System.Exception ex)
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
    
    public async Task<ResponseEntity> RemoveService(RemoveServiceCommand command)
    {
        try
        {
            // Check id exists
            var service = await _serviceRepository.GetByIdAsync(command.Id);
            if(service == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Id không tồn tại",
                    Data = null
                };
            }

            // Delete room
            await _serviceRepository.DeleteAsync(service);
            await _unitOfWork.SaveChangesAsync();
            
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Xóa dịch vụ thành công",
                Data = command
            };
        }
        catch (System.Exception ex)
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

    public async Task<ResponseEntity> UpdateServiceStatus(UpdateServiceStatusCommand command)
    {
        try
        {
            // Check id exists
            var service = await _serviceRepository.GetByIdAsync(command.Id);
            if(service == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Id không tồn tại",
                    Data = null
                };
            }
            
            //Kiểm tra trạng thái dịch vụ có thay đổi ko
            if(service.Status == command.Status.ToString())
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Trạng thái dịch vụ không thay đổi",
                    Data = null
                };
            }

            // Update service status
            service.Status = command.Status.ToString();

            await _serviceRepository.UpdateAsync(service);
            await _unitOfWork.SaveChangesAsync();
            
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Cập nhật trạng thái dịch vụ thành công",
                Data = command
            };
        }
        catch (System.Exception ex)
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

    public async Task<ResponseEntity> SearchService(SearchServicesQuery query)
    {
        try
        {
            // Validate pageIndex and pageSize
            if (query.PageIndex < 0)
            {
                query.PageIndex = 1;
            }
            if (query.PageSize < 0)
            {
                query.PageSize = 10;
            }

            // Search service
            var services = await _serviceRepository.SearchServiceAsync(query.Keyword, query.PageIndex, query.PageSize);

            if (services == null || !services.Any())
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Không tìm thấy dịch vụ",
                    Data = null
                };
            }

            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Tìm thấy dịch vụ",
                Data = services
            };
        }
        catch (System.Exception ex)
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

    // public async Task<ResponseEntity> SearchRoom(SeachRoomQuery query)
    // {
    //     try
    //     {
    //         // Validate pageIndex and pageSize
    //         if (query.PageIndex < 0)
    //         {
    //             query.PageIndex = 1;
    //         }
    //         if (query.PageSize < 0)
    //         {
    //             query.PageSize = 10;
    //         }

    //         // Search room
    //         var rooms = await _roomRepository.SearchRoomAsync(query.Keyword, query.PageIndex, query.PageSize);

    //         if (rooms == null || !rooms.Any())
    //         {
    //             return new ResponseEntity
    //             {
    //                 IsSuccess = false,
    //                 StatusCode = 404,
    //                 Message = "Không tìm thấy phòng",
    //                 Data = null
    //             };
    //         }
    //         return new ResponseEntity
    //         {
    //             IsSuccess = true,
    //             StatusCode = 200,
    //             Message = "Lấy danh sách phòng thành công",
    //             Data = rooms
    //         };
    //     }
    //     catch (Exception ex)
    //     {
    //         return new ResponseEntity
    //         {
    //             IsSuccess = false,
    //             StatusCode = 500,
    //             Message = ex.Message,
    //             Data = null
    //         };
    //     }
    // }


}