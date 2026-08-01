using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.RemoveCategory;

public class RemoveCategoryHandler : IRequestHandler<RemoveCategoryCommand, ResponseEntity>
{
    private readonly ICategoryService _serviceCategoryService;
    public RemoveCategoryHandler(ICategoryService serviceCategoryService)
    {
        _serviceCategoryService = serviceCategoryService;
    }
    public async Task<ResponseEntity> Handle(RemoveCategoryCommand request, CancellationToken cancellationToken)
    {
        return await _serviceCategoryService.DeleteCategory(request);
    }
}