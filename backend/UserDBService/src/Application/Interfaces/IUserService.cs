using Application.Commands.AddUser;
using Application.Queries.GetAllUser;
using Application.Queries.GetUserById;
using Share.CommonModel;
using UserDBService.Application.Commands.RemoveUser;
using UserDBService.Application.Commands.UpdateUser;

namespace Application.Interfaces;

public interface IUserService
{
    Task<ResponseEntity> GetAllUsers(GetAllUserQuery query); //xem danh sách
    Task<ResponseEntity> GetUserById(GetUserByIdQuery query); //xem chi tiết
    Task<ResponseEntity> CreateUser(AddUserCommand command); //thêm
    Task<ResponseEntity> UpdateUser(UpdateUserCommand command); //cập nhật
    Task<ResponseEntity> DeleteUser(RemoveUserCommand command); //xóa
}