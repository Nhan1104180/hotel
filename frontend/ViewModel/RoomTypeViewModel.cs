using System.ComponentModel.DataAnnotations;

public class RoomTypeViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Capacity { get; set; }
}

public class AddRoomTypeViewModel
{
    [Required(ErrorMessage = "Không được để trống")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Không được để trống")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Không được để trống")]
    public int Capacity { get; set; }
}

public class UpdateRoomTypeViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Không được để trống")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Không được để trống")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Không được để trống")]
    public int Capacity { get; set; }
}