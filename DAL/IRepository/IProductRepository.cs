namespace DAL.IRepository
{
    public interface IProductRepository
    {
        void CreateProduct();

        void UpdateProductById(Guid id);

        void GetAllProduct();

        void GetProductById(Guid id);
    }
}
