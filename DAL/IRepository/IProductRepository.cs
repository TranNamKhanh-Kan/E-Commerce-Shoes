using DAL.DTO;
using DAL.Entities;
using DAL.IRepository;

namespace DAL.IRepository
{
    public interface IProductRepository
    {
        Product CreateProduct(ProductRequest createProduct);
        Product UpdateProductById(Guid id, ProductRequest updateProduct);
        bool DeleteProduct(Guid id);
        List<Product> GetAllProduct();
        Product GetProductById(Guid id);
        List<Product> SearchProducts(string? keyword, string? type, string? status);
    }
}
