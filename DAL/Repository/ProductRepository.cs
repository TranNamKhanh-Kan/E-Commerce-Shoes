using DAL.DTO;
using DAL.Entities;
using DAL.IRepository;

namespace DAL.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ECommerceShoesContext _db;

        public ProductRepository(ECommerceShoesContext db)
        {
            _db = db;
        }
        public Product CreateProduct(ProductRequest createProduct)
        {
            var newProduct = new Product();
            newProduct.ProductId = Guid.NewGuid();
            newProduct.Name = createProduct.Name;
            newProduct.Type = createProduct.Type;
            newProduct.Quantity = createProduct.Quantity;
            newProduct.Price = createProduct.Price;
            newProduct.Size = createProduct.Size;
            newProduct.Status = createProduct.Status;
            newProduct.ImageUrl = createProduct.ImageUrl;
            _db.Products.Add(newProduct);
            _db.SaveChanges();
            return newProduct;
        }

        public List<Product> GetAllProduct()
        {
            return _db.Products.ToList();
        }

        public Product GetProductById(Guid id)
        {
            return _db.Products.SingleOrDefault(p => p.ProductId == id);
        }

        public Product UpdateProductById(Guid id, ProductRequest updateProduct)
        {
            var prod = _db.Products.SingleOrDefault(p => p.ProductId == id);
            prod.Type = updateProduct.Type;
            prod.Quantity = updateProduct.Quantity;
            prod.Price = updateProduct.Price;
            prod.Status = updateProduct.Status;
            prod.ImageUrl = updateProduct.ImageUrl;
            prod.Name = updateProduct.Name;
            _db.SaveChanges();
            return prod;
        }
    }
}
