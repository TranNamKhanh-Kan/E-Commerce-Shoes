using DAL.DTO;
using DAL.Entities;

namespace DAL.IRepository
{
    public interface IProductRepository
    {
        Product CreateProduct(ProductRequest createProduct);

        Product UpdateProductById(Guid id, ProductRequest updateProduct);

        List<Product> GetAllProduct();

        Product GetProductById(Guid id);
    }
}
