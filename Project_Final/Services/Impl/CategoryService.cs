using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project_Final.Context;
using Project_Final.ExceptionHandle;
using Project_Final.Models;
using Project_Final.Models.Entity;
using Project_Final.Models.Request;
using Project_Final.Models.Response;
using Project_Final.Utils;

namespace Project_Final.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        private DBContext dbContext;
        private readonly IMapper mapper;

        public CategoryService(DBContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public Response<List<Category>> create(List<string> categoryNames)
        {
            var categories = new List<Category>();
            if (categoryNames == null || categoryNames.Count == 0)
                return Response<List<Category>>.OfFailedFieldValidations(
                        new FieldValidationException([
                            new FieldValidation("name", "Không được phép để trống")]));
            foreach (var categoryName in categoryNames)
            {
                var name = categoryName?.Trim();
                if (name.IsNullOrEmpty())
                    return Response<List<Category>>.OfFailedFieldValidations(
                        new FieldValidationException([
                            new FieldValidation("name", "Không được phép để trống")]));
                var isExistCate = dbContext.Categories.Any(c => name.ToLower() == c.Name.ToLower());
                if (isExistCate)
                {
                    return Response<List<Category>>.OfFailedFieldValidations(
                        new FieldValidationException([
                            new FieldValidation("name", name + " đã tồn tại")]));
                }
                else
                {
                    var category = new Category { Name = name, Status = Status.ACTIVE };
                    dbContext.Categories.Add(category);
                    categories.Add(category);
                }
            }
            dbContext.SaveChanges();
            return Response<List<Category>>.OfSuccessed(categories);
        }

        public Response<Category> delete(long id)
        {
            var category = dbContext.Categories.Find(id);
            if (category == null)
                return Response<Category>.OfFailed(new BussinessException(ErrorCode.NOT_FOUND_DATA, "Danh mục không tồn tại"));
            var isExistProductOfCate = dbContext.Products.Any(p => p.CategoryId == id);
            if (isExistProductOfCate)
                return Response<Category>.OfFailed(new BussinessException(ErrorCode.INVALID_CATEGORY_ID, "Danh mục này không thể xóa"));
            dbContext.Categories.Remove(category);
            dbContext.SaveChanges();
            return Response<Category>.OfSuccessed();
        }

        public Response<List<Category>> getCategories(int page = 1, int size = 10)
        {
            var totalElemant = dbContext.Categories.Count();
            var categories = dbContext.Categories.AsQueryable()
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();
            if (categories == null || categories.Count == 0)
            {
                return Response<List<Category>>.OfSuccessed(new List<Category>(), page, size, totalElemant);
            }
            return Response<List<Category>>.OfSuccessed(categories, page, size, totalElemant);
        }

        public Response<Category> getCategory(long id)
        {
            return Response<Category>.OfSuccessed(dbContext.Categories.Find(id));
        }

        public Response<Category> update(UpdateCategoryRequest request)
        {
            if (request.Name.IsNullOrEmpty())
                return Response<Category>.OfFailedFieldValidations(
                        new FieldValidationException([
                            new FieldValidation("name", "Không được để trống")]));

            var category = dbContext.Categories.Find(request.Id);
            var isExistCate = dbContext.Categories.Any(c => c.Name == request.Name && c.Id != request.Id);
            if (isExistCate)
                return Response<Category>.OfFailedFieldValidations(new FieldValidationException([new FieldValidation("name", "Tên đã tồn tại")]));
            try
            {
                if (category == null)
                    return Response<Category>.OfFailed(new BussinessException(ErrorCode.NOT_FOUND_DATA, "Danh mục không tồn tại"));
                mapper.Map(request, category);
                dbContext.Update(category);
                dbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                return Response<Category>.OfFailed(new BussinessException(ErrorCode.DUPLICATE_CATEGORY, "Danh mục đã tồn tại"));
            }
            return Response<Category>.OfSuccessed(category);
        }
    }
}
