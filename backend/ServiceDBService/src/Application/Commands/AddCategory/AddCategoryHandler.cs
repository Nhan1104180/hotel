using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.AddCategory;

public class AddCategoryHandler : IRequestHandler<AddCategoryCommand, ResponseEntity>
{
    private readonly ICategoryService _categoryService;
    public AddCategoryHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    public async Task<ResponseEntity> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        return await _categoryService.AddServiceCategory(request);
    }
}