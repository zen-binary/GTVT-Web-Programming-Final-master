using Project_Final.Models;
using Project_Final.Models.Request;
using Project_Final.Models.Response;

namespace Project_Final.Services
{
    public interface IProductService
    {
        Response<List<ProductsResponse>> Create(CreateProductRequest request);
        Response<List<ProductsResponse>> GetProducts(string productName, long categoryId, Status status, int page, int size);
        Response<ProductsResponse> GetProductById(long id);
        Response<ProductsResponse> Update(UpdateProductRequest request);
        Response<ProductsResponse> Delete(long id);
    }
}
