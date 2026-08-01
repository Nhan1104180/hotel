using Application.Commands.AddRoomType;
using Application.Commands.RemoveRoomType;
using Application.Commands.UpdateRoomType;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.ValueObjects;
using Infrastructure.Util;
using RoomDBService.src.Domain.Entities;
using Share.CommonModel;

namespace Application.Services;

public class RoomTypeService : IRoomTypeService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly UnitOfWork _unitOfWork;
    public RoomTypeService(IRoomRepository roomRepository, IRoomTypeRepository roomTypeRepository, UnitOfWork unitOfWork)
    {
        _roomTypeRepository = roomTypeRepository;
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResponseEntity> GetAllRoomType()
    {
        try
        {
            var roomTypes = await _roomTypeRepository.GetAllRoomTypeAsync();
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Lấy danh sách tất cả các loại phòng thành công",
                Data = roomTypes
            };
        }
        catch (Exception ex)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message
            };
        }
    }

    public async Task<ResponseEntity> AddRoomType(AddRoomTypeCommand command)
    {
        try
        {
            //Kiểm tra tên loại phòng đã tồn tại chưa
            var existsRoomTypeName = await _roomTypeRepository.ExistsByNameAsync(command.Name);
            if (existsRoomTypeName)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Tên loại phòng đã tồn tại",
                    Data = null
                };
            }

            // ValueObjects
            var name = new Name(command.Name);
            var capacity = new Capacity(command.Capacity);
            var description = new Description(command.Description);

            // Create roomType
            var roomType = new RoomType
            {
                Name = name.Value,
                Description = description.Value,
                Capacity = capacity.Value
            };

            // Add roomType
            await _roomTypeRepository.AddRoomTypeAsync(roomType);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Thêm loại phòng thành công",
                Data = command
            };
        }
        catch (Exception ex)
        {
            return new ResponseEntity
            {
                Data = null,
                Message = ex.Message,
                StatusCode = 500
            };
        }
    }

    public async Task<ResponseEntity> UpdateRoomType(UpdateRoomTypeCommand command)
    {
        try
        {
            // Check roomType exists
            var roomType = await _roomTypeRepository.GetByIdAsync(command.Id);
            if (roomType == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Id không tồn tại",
                    Data = null
                };
            }

            // Check roomType name exists
            var existsRoomTypeName = await _roomTypeRepository.ExistsByNameAsync(command.Name,command.Id);
            if (existsRoomTypeName)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Tên loại phòng đã tồn tại",
                    Data = null
                };
            }

            // ValueObjects
            var name = new Name(command.Name);
            var capacity = new Capacity(command.Capacity);
            var description = new Description(command.Description);

            // Update roomType
            roomType.Name = name.Value;
            roomType.Capacity = capacity.Value;
            roomType.Description = description.Value;

            // Update roomType
            await _roomTypeRepository.UpdateAsync(roomType);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Cập nhật loại phòng thành công",
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
            };
        }
    }
    
    public async Task<ResponseEntity> RemoveRoomType(RemoveRoomTypeCommand command)
    {
        try
        {
            //Kiểm tra room type có tồn tại không
            var roomType = await _roomTypeRepository.GetByIdAsync(command.Id);
            if (roomType == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Không tìm thấy loại phòng",
                    Data = null
                };
            }

            //Kiểm tra room type có tồn tại trong room không
            var rooms = await _roomRepository.WhereAsync(x => x.RoomTypeId == command.Id);

            if (rooms.Any())
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Không thể xóa loại phòng vì có phòng đang sử dụng",
                    Data = null
                };
            }

            await _roomTypeRepository.DeleteAsync(roomType);
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Xóa loại phòng thành công",
                Data = null
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
}