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
            var newProduct = new Product
            {
                ProductId = Guid.NewGuid(),
                Name = createProduct.Name,
                Type = createProduct.Type,
                Quantity = createProduct.Quantity,
                Price = createProduct.Price,
                Size = createProduct.Size,
                Status = createProduct.Status,
                ImageUrl = createProduct.ImageUrl
            };
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
            if (prod == null) return null;

            prod.Type = updateProduct.Type;
            prod.Quantity = updateProduct.Quantity;
            prod.Price = updateProduct.Price;
            prod.Size = updateProduct.Size;
            prod.Status = updateProduct.Status;
            prod.ImageUrl = updateProduct.ImageUrl;
            prod.Name = updateProduct.Name;
            _db.SaveChanges();
            return prod;
        }

        public bool DeleteProduct(Guid id)
        {
            var prod = _db.Products.SingleOrDefault(p => p.ProductId == id);
            if (prod == null) return false;

            var inOrder = _db.OrderDetails.Any(od => od.ProductId == id);
            if (inOrder)
            {
                prod.Status = "Inactive";
                _db.SaveChanges();
                return true;
            }

            _db.Products.Remove(prod);
            _db.SaveChanges();
            return true;
        }

        public List<Product> SearchProducts(string? keyword, string? type, string? status)
        {
            var query = _db.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var key = keyword.Trim().ToLower();
                query = query.Where(p =>
                    (p.Name != null && p.Name.ToLower().Contains(key)) ||
                    (p.Type != null && p.Type.ToLower().Contains(key)));
            }

            if (!string.IsNullOrWhiteSpace(type))
                query = query.Where(p => p.Type == type);

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(p => p.Status == status);

            return query.ToList();
        }
    }
}
