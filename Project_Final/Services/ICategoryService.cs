using Project_Final.Models.Entity;
using Project_Final.Models.Request;
using Project_Final.Models.Response;

namespace Project_Final.Services
{
    public interface ICategoryService
    {
        Response<List<Category>> create(List<string> categoryNames);
        Response<Category> getCategory(long id);
        Response<List<Category>> getCategories(int page, int size);
        Response<Category> update(UpdateCategoryRequest request);
        Response<Category> delete(long id);
    }
}
