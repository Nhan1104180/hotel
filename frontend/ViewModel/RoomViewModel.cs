using System.ComponentModel.DataAnnotations;

namespace frontend.ViewModel;

public class RoomViewModel
{
    public int Id { get; set; }
    public int RoomTypeId { get; set; }
    public string RoomNumber { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; }
    public string RoomType { get; set; }
    public string Description { get; set; }
    public int Capacity { get; set; }
    public string ImageUrl { get; set; }
}

public class AddRoomViewModel
{
    [Required(ErrorMessage = "Không được để trống")]
    [RegularExpression(@"^[A-Z][0-9]{3}$", ErrorMessage = "Room Number phải có dạng A001, A101, B205,... Chữ cái đầu phải viết hoa và theo sau là đúng 3 chữ số.")]
    public string RoomNumber { get; set; }

    [Required(ErrorMessage = "Không được để trống")]
    public int RoomTypeId { get; set; }

    [Required(ErrorMessage = "Không được để trống")]
    [Range(1000, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 1000")]
    [RegularExpression(@"^[1-9][0-9]*000$", ErrorMessage = "Giá phải có dạng 1000, 2000, 5000, 100000, 500000... (không có 5125, 123987, 500123, 500001)")]
    public decimal Price { get; set; }
}

public class UpdateRoomViewModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Không được để trống")]
    public int RoomTypeId { get; set; }

    [Required(ErrorMessage = "Không được để trống")]
    [Range(1000, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 1000")]
    [RegularExpression(@"^[1-9][0-9]*000$", ErrorMessage = "Giá phải có dạng 1000, 2000, 5000, 100000, 500000... (không có 5125, 123987, 500123, 500001)")]
    public decimal Price { get; set; }
}

public class RoomStatusViewModel
{
    public int Id { get; set; }
    public string name { get; set; }
}

public class UpdateRoomStatusViewModel
{
    public int Id { get; set; }
    public int Status { get; set; }
}
