using DAL.DTO;
using DAL.Entities;

namespace BAL.IService
{
    public interface IProductService
    {
        Product CreateProduct(ProductRequest createProduct);
        Product UpdateProductById(Guid id, ProductRequest updateProduct);
        bool DeleteProduct(Guid id);
        List<Product> GetAllProduct();
        Product GetProductById(Guid id);
        List<Product> SearchProducts(string? keyword, string? type, string? status);
    }
}
