using Application.Commands.AddRoom;
using Application.Commands.RemoveRoom;
using Application.Commands.UpdateRoom;
using Application.Commands.UpdateRoomStatus;
using Application.Interfaces;
using Application.Queries.GetAvailableRooms;
using Application.Queries.GetRoomById;
using Application.Queries.SeachRoom;
using Domain.Enums;
using Domain.Interfaces;
using Domain.ValueObjects;
using Infrastructure.Util;
using RoomDBService.src.Domain.Entities;
using Share.CommonModel;

namespace Application.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IRoomImageRepository _roomImageRepository;
    // private readonly IBookingRepository _bookingRepository;
    private readonly UnitOfWork _unitOfWork;
    public RoomService(IRoomRepository roomRepository, IRoomTypeRepository roomTypeRepository, IRoomImageRepository roomImageRepository, UnitOfWork unitOfWork) //IBookingRepository bookingRepository, UnitOfWork unitOfWork)
    {
        _roomRepository = roomRepository;
        _roomTypeRepository = roomTypeRepository;
        _roomImageRepository = roomImageRepository;
        // _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseEntity> GetAllRooms()
    {
        try
        {
            var rooms = await _roomRepository.GetAllRoomsAsync();
            if (rooms == null || !rooms.Any())
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 200,
                    Message = "Không tìm thấy phòng",
                    Data = null,
                };
            }
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Lấy danh sách phòng thành công",
                Data = rooms
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

    public async Task<ResponseEntity> GetRoomById(GetRoomByIdQuery query)
    {
        //Get room by Id
        var user = await _roomRepository.GetRoomByIdAsync(query.Id);

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

    public async Task<ResponseEntity> AddRoom(AddRoomCommand command)
    {
        try
        {
            // Check roomNumber exists
            var existsRoomNumber = await _roomRepository.ExistsByRoomNumberAsync(command.RoomNumber);

            if (existsRoomNumber)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Số phòng đã tồn tại",
                    Data = null
                };
            }

            // Check RoomType exists
            var RoomType = await _roomTypeRepository.GetByIdAsync(command.RoomTypeId);
            if (RoomType == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Loại phòng không tồn tại",
                    Data = null
                };
            }

            // ValueObjects
            var roomNumber = new RoomNumber(command.RoomNumber);
            var money = new Money(command.Price);

            // Create room
            var room = new Room
            {
                RoomNumber = roomNumber.Value,
                RoomTypeId = command.RoomTypeId,
                Status = RoomStatus.Available.ToString(),
                Price = money.Amount,
            };

            // Add room
            await _roomRepository.AddRoomAsync(room);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Thêm phòng thành công",
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

    public async Task<ResponseEntity> UpdateRoom(UpdateRoomCommand command)
    {
        try
        {
            // Check room exists
            var room = await _roomRepository.GetByIdAsync(command.Id);
            if (room == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Id không tồn tại",
                    Data = null
                };
            }

            // Check RoomType exists
            var RoomType = await _roomTypeRepository.GetByIdAsync(command.RoomTypeId);
            if (RoomType == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Loại phòng không tồn tại",
                    Data = null
                };
            }

            // ValueObjects
            var money = new Money(command.Price);

            // Update room
            room.RoomTypeId = command.RoomTypeId;
            room.Price = money.Amount;

            // Update room
            await _roomRepository.UpdateAsync(room);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Cập nhật phòng thành công",
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

    public async Task<ResponseEntity> RemoveRoom(RemoveRoomCommand command)
    {
        try
        {
            // Check id exists
            var room = await _roomRepository.GetByIdAsync(command.Id);
            if (room == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Id không tồn tại",
                    Data = null
                };
            }

            //Kiểm tra booking có tồn tại không
            // var booking = await _bookingRepository.WhereAsync(x => x.RoomId == command.Id);
            // if (booking.Any())
            // {
            //     return new ResponseEntity
            //     {
            //         IsSuccess = false,
            //         StatusCode = 400,
            //         Message = "Phòng đã có lịch sử đặt phòng,không thể xóa",
            //         Data = null
            //     };
            // }

            // Delete room
            await _roomRepository.DeleteAsync(room);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Xóa phòng thành công",
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

    public async Task<ResponseEntity> SearchRoom(SeachRoomQuery query)
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

            // Search room
            var rooms = await _roomRepository.SearchRoomAsync(query.Keyword, query.PageIndex, query.PageSize);

            if (rooms == null || !rooms.Any())
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Không tìm thấy phòng",
                    Data = null
                };
            }
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Lấy danh sách phòng thành công",
                Data = rooms
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

    public async Task<ResponseEntity> GetAvailableRooms(GetAvailableRoomsQuery query)
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

            // Get available rooms
            var rooms = await _roomRepository.GetAvailableRoomsAsync(query.PageIndex, query.PageSize);

            if (rooms == null || !rooms.Any())
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Không tìm thấy phòng trống",
                    Data = null
                };
            }
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Lấy danh sách phòng trống thành công",
                Data = rooms
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

    public async Task<ResponseEntity> UpdateRoomStatus(UpdateRoomStatusCommand command)
    {
        try
        {
            // Get room by Id
            var room = await _roomRepository.GetRoomEntityByIdAsync(command.Id);

            // Kiểm tra room có tồn tại ko
            if (room == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Không tìm thấy dữ liệu",
                    Data = null,
                };
            }

            //Kiểm tra trạng thái phòng có thay đổi ko
            if (room.Status == command.Status.ToString())
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Trạng thái phòng không thay đổi",
                    Data = null
                };
            }

            // Update room status
            room.Status = command.Status.ToString();

            // Update room
            await _roomRepository.UpdateAsync(room);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Cập nhật trạng thái phòng thành công",
                Data = room
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