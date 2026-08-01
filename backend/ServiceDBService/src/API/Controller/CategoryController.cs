using Application.Commands.AddCategory;
using Application.Commands.RemoveCategory;
using Application.Commands.UpdateCategory;
using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryService categoryService, IMediator mediator, IMapper mapper)
        {
            _categoryService = categoryService;
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("GetAllCategory")]
        [SwaggerOperation(Summary = "Lấy tất cả danh mục")]
        public async Task<ActionResult> GetAllCategory()
        {
            var response = await _categoryService.GetAllCategory();
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("AddCategory")]
        [SwaggerOperation(Summary = "Thêm danh mục")]
        public async Task<ActionResult> AddCategory([FromBody] AddCategoryDTO category)
        {
            var command = _mapper.Map<AddCategoryCommand>(category);
           
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("UpdateCategory/{id}")]
        [SwaggerOperation(Summary = "Cập nhật danh mục")]
        public async Task<ActionResult> UpdateCategory([FromRoute] int id , [FromBody] UpdateCategoryDTO category)
        {
            var command = _mapper.Map<UpdateCategoryCommand>(category);
            command.Id = id;
            
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("DeleteCategory/{id}")]
        [SwaggerOperation(Summary = "Xóa danh mục")]
        public async Task<ActionResult> DeleteCategory([FromRoute] int id)
        {
            var command = new RemoveCategoryCommand
            {
                Id = id
            };
            
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
    }
}