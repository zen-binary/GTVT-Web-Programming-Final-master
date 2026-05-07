using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project_Final.Context;
using Project_Final.ExceptionHandle;
using Project_Final.Models;
using Project_Final.Models.Entity;
using Project_Final.Models.Request;
using Project_Final.Models.Response;
using Project_Final.Utils;
using System.Text.Json;

namespace Project_Final.Services.Impl
{
    public class ProductService : IProductService
    {
        private DBContext dbContext;
        private readonly IMapper mapper;
        public ProductService(DBContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public Response<List<ProductsResponse>> Create(CreateProductRequest request)
        {
            if (request.Name.IsNullOrEmpty())
            {
                return Response<List<ProductsResponse>>.OfFailedFieldValidations(new FieldValidationException([new FieldValidation("name", "Không được để trống")]));
            }
            List<ProductsResponse> productResponses = new List<ProductsResponse>();
            var categoty = dbContext.Categories.Find(request.CategoryId);
            if (categoty == null || categoty.Status == Status.INACTIVE)
                return Response<List<ProductsResponse>>.OfFailedFieldValidations(new FieldValidationException([new FieldValidation("categoryId", "Không tồn tại")]));
            int i = 1;
            foreach (var detail in request.Details)
            {
                var fieldValidations = validateCreateProduct(request.Name, detail, i);
                if (fieldValidations != null && fieldValidations.Count > 0)
                {
                    return Response<List<ProductsResponse>>.OfFailedFieldValidations(new FieldValidationException(fieldValidations));
                }
                var product = mapper.Map<Product>(request);
                var entity = mapper.Map(detail, product);
                dbContext.Products.Add(entity);
                productResponses.Add(mapper.Map<ProductsResponse>(entity));
                i++;
            }
            dbContext.SaveChanges();
            return Response<List<ProductsResponse>>.OfSuccessed(productResponses);
        }

        public Response<ProductsResponse> Delete(long id)
        {
            var product = dbContext.Products.Find(id);
            if (product == null)
            {
                return Response<ProductsResponse>.OfFailed(ErrorCode.NOT_FOUND_DATA, "Not found product by id " + id + " to delete");
            }
            dbContext.Products.Remove(product);
            dbContext.SaveChanges();
            return Response<ProductsResponse>.OfSuccessed();
        }

        public Response<ProductsResponse> GetProductById(long id)
        {
            var productResponses = from p in dbContext.Products
                         join c in dbContext.Categories on p.CategoryId equals c.Id
                         select new ProductsResponse
                         {
                             Id = p.Id,
                             CategoryId = p.CategoryId,
                             CategoryName = c.Name,
                             Name = string.Join("-", p.Name, p.Size, p.Color),
                             Quantity = p.Quantity,
                             Price = p.Price,
                             Size = p.Size,
                             Color = p.Color,
                             Status = p.Status
                         };
            if (productResponses.Count() == 0)
            {
                return Response<ProductsResponse>.OfFailed(new BussinessException(ErrorCode.NOT_FOUND_DATA, "Not found product with id " + id));
            }
            return Response<ProductsResponse>.OfSuccessed(productResponses.FirstOrDefault());
        }

        public Response<List<ProductsResponse>> GetProducts(string? productName, long categoryId, Status status, int page = 1, int size = 10)
        {
            var query = from p in dbContext.Products
                        join c in dbContext.Categories on p.CategoryId equals c.Id
                        select new ProductsResponse
                        {
                            Id = p.Id,
                            CategoryId = p.CategoryId,
                            CategoryName = c.Name,
                            Name = string.Join("-", p.Name, p.Size, p.Color),
                            Quantity = p.Quantity,
                            Price = p.Price,
                            Size = p.Size,
                            Color = p.Color,
                            Status = p.Status
                        };
            if (productName != null)
            {
                query = query.Where(p => p.Name.Contains(productName));
            }
            if (categoryId > 0)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }
            if (status != Status.ALL)
            {
                query = query.Where(p => p.Status == status);
            }
            var totalElemant = query.Count();
            var products = query
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();
            if (products == null || products.Count == 0)
            {
                return Response<List<ProductsResponse>>.OfFailed(new BussinessException(ErrorCode.NOT_FOUND_DATA, "Not found products"));
            }
            return Response<List<ProductsResponse>>.OfSuccessed(products, page, size, totalElemant);
        }

        public Response<ProductsResponse> Update(UpdateProductRequest request)
        {
            var product = dbContext.Products.Find(request.Id);
            if (product == null)
            {
                return Response<ProductsResponse>.OfFailed(new BussinessException(ErrorCode.NOT_FOUND_DATA, "Not found product with id " + request.Id));
            }
            if (request.Quantity < product.Quantity)
            {
                return Response<ProductsResponse>.OfFailedFieldValidations(new FieldValidationException([new FieldValidation("Quantity", "Quantity can't decrease manual")]));
            }
            if(request.Quantity == 0)
            {
                request.Quantity = product.Quantity;
            }
            if(request.CategoryId == 0)
            {
                request.CategoryId = product.CategoryId;
            } else
            {
                var IsExistCate = dbContext.Categories.Any(c => c.Id == request.CategoryId && c.Status == Status.ACTIVE);
                if (!IsExistCate)
                    return Response<ProductsResponse>.OfFailed(new BussinessException(ErrorCode.INVALID_CATEGORY_ID, "CategoryId " + request.CategoryId + " không hợp lệ"));
            }
            if(request.Price == 0)
                request.Price = product.Price;
            product.LastModifiedBy = "system";
            var productNew = mapper.Map(request, product);
            dbContext.SaveChanges();
            return Response<ProductsResponse>.OfSuccessed(mapper.Map<ProductsResponse>(productNew));
        }

        private List<FieldValidation> validateCreateProduct(string name, ProductAttribute attribute, int index)
        {
            List<FieldValidation> fieldValidations = new List<FieldValidation>();
            if (attribute.Size.IsNullOrEmpty())
                fieldValidations.Add(new FieldValidation("size-"+ index, "Không được phép null hoặc rỗng"));
            if (attribute.Color.IsNullOrEmpty())
                fieldValidations.Add(new FieldValidation("color-" + index, "Không được phép null hoặc rỗng"));
            var isExistProduct = dbContext.Products.Any(p => p.Name == name && p.Size == attribute.Size && p.Color == attribute.Color);
            if (isExistProduct)
            {
                fieldValidations.Add(new FieldValidation("product-" + index, "Sản phẩm đã tồn tại"));
            }
            if (attribute.Quantity < 0)
            {
                fieldValidations.Add(new FieldValidation("quantity-" + index, "Không được < 0"));
            }
            if (attribute.Price < 0)
            {
                fieldValidations.Add(new FieldValidation("price", "Không được < 0"));
            }
            return fieldValidations;
        }
    }
}
