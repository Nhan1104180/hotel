using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.UpdateCategory;

public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, ResponseEntity>
{
    private readonly ICategoryService _serviceCategoryService;

    public UpdateCategoryHandler(ICategoryService serviceCategoryService)
    {
        _serviceCategoryService = serviceCategoryService;
    }

    public async Task<ResponseEntity> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        return await _serviceCategoryService.UpdateCategory(request);
    }
}