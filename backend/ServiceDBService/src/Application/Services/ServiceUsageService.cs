using Application.Commands.AddServiceUsage;
using Application.Commands.RemoveServiceUsage;
using Application.DTO;
using Application.Interfaces;
using Application.Queries.GetServiceUsageByBooking;
using AutoMapper;
using BookingDBService.Domain.Interfaces;
using Domain.Enums;
using Domain.Interfaces;
using Domain.ValueObjects;
using Infrastructure.Util;
using ServiceDBService.src.Domain.Entities;
using Share.CommonModel;

namespace Application.Services;

public class ServiceUsageService : IServiceUsageService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IServiceUsageRepository _serviceUsageRepository;
    private readonly IMapper _mapper;
    private readonly UnitOfWork _unitOfWork;
    public ServiceUsageService(IBookingRepository bookingRepository, IServiceRepository serviceRepository, IServiceUsageRepository serviceUsageRepository, IMapper mapper, UnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _serviceRepository = serviceRepository;
        _serviceUsageRepository = serviceUsageRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseEntity> GetServiceUsageByBookingId(GetServiceUsageByBookingQuery query)
    {
        try
        {
            // Kiểm tra Booking tồn tại
            var booking = await _bookingRepository.GetByIdAsync(query.BookingId);
            if(booking == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Booking không tồn tại",
                    Data = null
                };
            }

            var serviceUsages = await _serviceUsageRepository.GetServiceUsageByBookingIdAsync(query.BookingId);
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Lấy danh sách dịch vụ thành công",
                Data = _mapper.Map<List<ServiceUsageDTO>>(serviceUsages)
            };
        }
        catch (Exception ex)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = "Lỗi khi lấy danh sách dịch vụ: " + ex.Message,
                Data = null
            };
        }
    }

    public async Task<ResponseEntity> AddServiceUsage(AddServiceUsageCommand command)
    {
        try
        {
            // 1. Kiểm tra Booking tồn tại
            var booking = await _bookingRepository.GetByIdAsync(command.BookingId);
            if(booking == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Booking không tồn tại",
                    Data = null
                };
            }

            // 2. Kiểm tra Service tồn tại
            var service = await _serviceRepository.GetByIdAsync(command.ServiceId);
            if(service == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Service không tồn tại",
                    Data = null
                };
            }

            // 3. Kiểm tra đã thêm dịch vụ chưa
            var exists = await _serviceUsageRepository.ExistsAsync(command.BookingId, command.ServiceId);
            if(exists)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Service already added",
                    Data = null
                };
            }

            //Kiểm tra Booking có hủy chưa nếu có thì không cho thêm dịch vụ
            if(booking.Status == BookingStatus.Cancelled.ToString())
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Booking đã bị hủy",
                    Data = null
                };
            }

            // 4. Value Objects
            var quantity = new Quantity(command.Quantity);
            var price = new Money(service.Price);

            //5. Tính tổng tiền
            decimal totalPrice = quantity.Value * price.Value;

            // 6. Create ServiceUsage
            var serviceUsage = new ServiceUsage
            {
                BookingId = command.BookingId,
                ServiceId = command.ServiceId,
                Quantity = quantity.Value,
                UnitPrice = price.Value,
                TotalAmount = totalPrice,
                CreatedAt = DateTime.Now
            };

            // 7. Add ServiceUsage
            await _serviceUsageRepository.AddServiceUsageAsync(serviceUsage);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Thêm dịch vụ thành công",
                Data = _mapper.Map<ServiceUsageDTO>(serviceUsage)
            };
        }
        catch (Exception ex)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = "Lỗi khi thêm dịch vụ: " + ex.Message,
                Data = null
            };
        }
    }

    public async Task<ResponseEntity> DeleteServiceUsage(RemoveServiceUsageCommand command)
    {
        try
        {
            // 1. Kiểm tra Booking tồn tại
            var booking = await _bookingRepository.GetByIdAsync(command.BookingId);
            if(booking == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Booking không tồn tại",
                    Data = null
                };
            }

            // 2. Kiểm tra Service tồn tại
            var service = await _serviceRepository.GetByIdAsync(command.ServiceId);
            if(service == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Service không tồn tại",
                    Data = null
                };
            }

            // 3. Kiểm tra đã thêm dịch vụ chưa
            var serviceUsage = await _serviceUsageRepository.GetAsync(command.BookingId, command.ServiceId);
            if(serviceUsage == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Service chưa được thêm vào booking",
                    Data = null
                };
            }

            // 4. Delete ServiceUsage
            await _serviceUsageRepository.DeleteServiceUsageAsync(serviceUsage);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Xóa dịch vụ thành công",
                Data = null
            };
        }
        catch (Exception ex)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = "Lỗi khi xóa dịch vụ: " + ex.Message,
                Data = null
            };
        }
    }
}