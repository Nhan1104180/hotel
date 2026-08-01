using Domain.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetAllUser;

public class GetAllUserHandler : IRequestHandler<GetAllUserQuery, ResponseEntity>
{
    private readonly IUserRepository _userRepository;
    public GetAllUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<ResponseEntity> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        var users =  await _userRepository.GetAllUsers(request.pageIndex, request.PageSize);

        return new ResponseEntity
        {
            IsSuccess = true,
            StatusCode = 200,
            Message = "Success",
            Data = users
        };
    }

}