using BAL.IService;
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
        public void CreateProduct()
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAllProduct()
        {
            return _repo.GetAllProduct();
        }

        public void GetProductById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void UpdateProductById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
