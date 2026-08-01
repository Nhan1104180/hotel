using Domain.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetUserById;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, ResponseEntity>
{
    private readonly IUserRepository _userRepository;
    public GetUserByIdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<ResponseEntity> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserById(request.Id);
        
        if(user == null)
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
}