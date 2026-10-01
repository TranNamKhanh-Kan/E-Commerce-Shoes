using BAL.IService;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DAL.DTO;
using DAL.Entities;
using DAL.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BAL.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly ICloudinary _cloudinary;
        private readonly string _folder;

        public ProductService(IProductRepository repo, ICloudinary cloudinary, IConfiguration configuration)
        {
            _repo = repo;
            _cloudinary = cloudinary;
            _folder = configuration["Cloudinary:Folder"] ?? "e-commerce-shoes";
        }

        public async Task<Product> CreateProduct(ProductRequest createProduct, IFormFile? image)
        {
            if (image == null || image.Length == 0)
                throw new InvalidOperationException("Product image is required.");
            var upload = await UploadImageAsync(image);
            createProduct.ImageUrl = upload.url;
            createProduct.PublicId = upload.publicId;
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

        public async Task<Product?> UpdateProductById(Guid id, ProductRequest updateProduct, IFormFile? image)
        {
            var existing = _repo.GetProductById(id);
            if (existing == null) return null;

            if (image != null && image.Length > 0)
            {
                var pubId = existing.PublicId;
                var upload = await UploadImageAsync(image);
                await _cloudinary.DestroyAsync(new DeletionParams(pubId) { ResourceType = ResourceType.Image, Type = "upload"});
                updateProduct.ImageUrl = upload.url;
                updateProduct.PublicId = upload.publicId;
            }
            else
            {
                updateProduct.ImageUrl = existing.ImageUrl;
            }

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

        private async Task<(string url, string publicId)> UploadImageAsync(IFormFile file)
        {
            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = _folder,
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false
            };

            var result = await _cloudinary.UploadAsync(uploadParams);
            if (result.Error != null)
                throw new InvalidOperationException($"Cloudinary upload failed: {result.Error.Message}");

            return (result.SecureUrl.ToString(), result.PublicId);
        }
    }
}
