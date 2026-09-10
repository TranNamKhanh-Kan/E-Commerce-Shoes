using DAL.Entities;

namespace BAL.IService
{
    public interface IProductService
    {
        void CreateProduct();

        void UpdateProductById(Guid id);

        List<Product> GetAllProduct();

        void GetProductById(Guid id);
    }
}
