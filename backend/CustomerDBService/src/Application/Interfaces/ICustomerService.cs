using Application.Commands.AddCustomer;
using Application.Commands.RemoveCustomer;
using Application.Commands.UpdateCustomer;
using Application.Queries.GetAllCustomer;
using Application.Queries.GetCustomerById;
using Share.CommonModel;

namespace Application.Interfaces;

public interface ICustomerService
{
    Task<ResponseEntity> GetAllCustomer(GetAllCustomerQuery query); //xem danh sách
    Task<ResponseEntity> GetCustomerById(GetCustomerByIdQuery query); //xem chi tiết
    Task<ResponseEntity> CreateCustomer(AddCustomerCommand command); //thêm
    Task<ResponseEntity> UpdateCustomer(UpdateCustomerCommand command); //cập nhật
    Task<ResponseEntity> DeleteCustomer(RemoveCustomerCommand command); //xóa
}