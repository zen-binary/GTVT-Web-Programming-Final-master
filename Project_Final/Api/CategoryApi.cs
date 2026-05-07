using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Final.Models.Entity;
using Project_Final.Models.Request;
using Project_Final.Models.Response;
using Project_Final.Services;

namespace Project_Final.Api
{
    //[Authorize]
    [ApiController]
    [Route("/admin/api/category")]
    public class CategoryApi : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryApi(ICategoryService category)
        {
            categoryService = category;
        }

        [HttpPost]
        public Response<List<Category>> Create(List<string> categories)
        {
            return categoryService.create(categories);
        }

        [HttpPut("{id}")]
        public Response<Category> Update(long id, UpdateCategoryRequest request)
        {
            request.Id = id;
            return categoryService.update(request);
        }

        [HttpGet]
        public Response<List<Category>> Gets([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            return categoryService.getCategories(page, size);
        }

        [HttpGet("{id}")]
        public Response<Category> Get(long id)
        {
            return categoryService.getCategory(id);
        }

        [HttpDelete("{id}")]
        public Response<Category> Delete(long id) {
            return categoryService.delete(id);
        }
    }
}
