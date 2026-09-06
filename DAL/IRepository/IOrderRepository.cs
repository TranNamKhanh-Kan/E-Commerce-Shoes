namespace DAL.IRepository
{
    public interface IOrderRepository
    {
        void CreateOrder();
        void UpdateOrder();
        void GetAllOrderById(Guid id);
    }
}
