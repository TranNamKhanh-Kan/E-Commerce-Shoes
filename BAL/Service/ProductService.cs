using BAL.IService;
using DAL.DTO;
using DAL.Entities;
using DAL.IRepository;

namespace BAL.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public Product CreateProduct(ProductRequest createProduct)
        {
            return _repo.CreateProduct(createProduct);
        }

        public List<Product> GetAllProduct()
        {
            return _repo.GetAllProduct();
        }

        public Product GetProductById(Guid id)
        {
            return _repo.GetProductById(id);
        }

        public Product UpdateProductById(Guid id, ProductRequest updateProduct)
        {
            return _repo.UpdateProductById(id, updateProduct);
        }

        public bool DeleteProduct(Guid id)
        {
            return _repo.DeleteProduct(id);
        }

        public List<Product> SearchProducts(string? keyword, string? type, string? status)
        {
            return _repo.SearchProducts(keyword, type, status);
        }
    }
}
