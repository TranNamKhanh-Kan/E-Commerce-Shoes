using DAL.DTO;
using DAL.Entities;
using Microsoft.AspNetCore.Http;

namespace BAL.IService
{
    public interface IProductService
    {
        Task<Product> CreateProduct(ProductRequest createProduct, IFormFile? image);
        Task<Product?> UpdateProductById(Guid id, ProductRequest updateProduct, IFormFile? image);
        bool DeleteProduct(Guid id);
        List<Product> GetAllProduct();
        Product GetProductById(Guid id);
        List<Product> SearchProducts(string? keyword, string? type, string? status);
    }
}
