using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Final.Models;
using Project_Final.Models.Request;
using Project_Final.Models.Response;
using Project_Final.Services;

namespace Project_Final.Api
{
    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("admin/api/product")]
    public class ProductApi : ControllerBase
    {
        private readonly IProductService productService;

        public ProductApi(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public Response<List<ProductsResponse>> GetAll([FromQuery] string? productName, [FromQuery] long categoryId, [FromQuery] Status status = Status.ALL, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            return productService.GetProducts(productName, categoryId, status, page, size);
        }
        [HttpGet("{id}")]
        public Response<ProductsResponse> Get(long id)
        {
            return productService.GetProductById(id);
        }

        [HttpPost]
        public Response<List<ProductsResponse>> Store(CreateProductRequest request)
        {
            return productService.Create(request);
        }

        [HttpPut("{id}")]
        public Response<ProductsResponse> Update(long id, UpdateProductRequest request)
        {
            request.Id = id;
            return productService.Update(request);
        }
    }
}
