namespace Domain.Enums;

public enum RoomStatus
{
    Available = 1,//Phòng trống
    Occupied = 2,//Phòng có khách
    Cleaning = 3,//Phòng đang dọn dẹp
    Maintenance = 4,//Phòng đang bảo trì
    OutOfService = 5,//Phòng không sử dụng
}