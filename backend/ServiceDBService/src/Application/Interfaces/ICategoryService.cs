using Application.Commands.AddCategory;
using Application.Commands.RemoveCategory;
using Application.Commands.UpdateCategory;
using Share.CommonModel;

namespace Application.Interfaces;

public interface ICategoryService
{
    Task<ResponseEntity> GetAllCategory();
    Task<ResponseEntity> AddServiceCategory(AddCategoryCommand command);
    Task<ResponseEntity> UpdateCategory(UpdateCategoryCommand command);
    Task<ResponseEntity> DeleteCategory(RemoveCategoryCommand command);
}
