using Application.Commands.AddCategory;
using Application.Commands.RemoveCategory;
using Application.Commands.UpdateCategory;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.ValueObjects;
using Infrastructure.Util;
using ServiceDBService.src.Domain.Entities;
using Share.CommonModel;

namespace Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IServiceCategoryRepository _serviceCategoryRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly UnitOfWork _unitOfWork;
    public CategoryService(IServiceCategoryRepository serviceCategoryRepository, IServiceRepository serviceRepository, UnitOfWork unitOfWork)
    {
        _serviceCategoryRepository = serviceCategoryRepository;
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseEntity> GetAllCategory()
    {
        try
        {
            var categories = await _serviceCategoryRepository.GetAllCategoryAsync();
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Lấy danh mục dịch vụ thành công",
                Data = categories
            };
        }catch(Exception ex)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
                Data = null
            };
        }
    }

    public async Task<ResponseEntity> AddServiceCategory(AddCategoryCommand command)
    {
        try
        {
            var existCategory = await _serviceCategoryRepository.ExistsByNameAsync(command.Name);
            if (existCategory)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Danh mục dịch vụ đã tồn tại",
                    Data = null
                };
            }
            var categoryName = new ServiceCategoryName(command.Name);
            var description = new Description(command.Description);
            
            var category = new ServiceCategory
            {
                Name = categoryName.Value,
                Description = description.Value,
                ImageUrl = command.ImageUrl
            };
            
            await _serviceCategoryRepository.AddCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Thêm danh mục dịch vụ thành công",
                Data = category
            };
        }catch(Exception ex)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
                Data = null
            };
        }
    }

    public async Task<ResponseEntity> UpdateCategory(UpdateCategoryCommand command)
    {
        try
        {
            var category = await _serviceCategoryRepository.GetByIdAsync(command.Id);
            if (category == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Id không tồn tại",
                    Data = null
                };
            }
            
            var categoryName = new ServiceCategoryName(command.Name);
            var description = new Description(command.Description);
            
            category.Name = categoryName.Value;
            category.Description = description.Value;
            category.ImageUrl = command.ImageUrl;
            
            //Update Caterory
            await _serviceCategoryRepository.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Cập nhật danh mục dịch vụ thành công",
                Data = category
            };
        }
        catch(Exception ex)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
                Data = null
            };
        }
    }

    public async Task<ResponseEntity> DeleteCategory(RemoveCategoryCommand command)
    {
        try
        {
            var category = await _serviceCategoryRepository.GetByIdAsync(command.Id);
            if (category == null)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Id không tồn tại",
                    Data = null
                };
            }

            var hasServices = await _serviceRepository.AnyAsync(x => x.CategoryId == command.Id);
            if (hasServices)
            {
                return new ResponseEntity
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Không thể xóa danh mục vì vẫn còn dịch vụ đang thuộc danh mục này. Vui lòng chuyển các dịch vụ sang danh mục khác hoặc xóa chúng trước.",
                    Data = null
                };
            }
            
            await _serviceCategoryRepository.DeleteAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseEntity
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Xóa danh mục dịch vụ thành công",
                Data = category
            };
        }
        catch(Exception ex)
        {
            return new ResponseEntity
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
                Data = null
            };
        }
    }
}