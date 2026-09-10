namespace BAL.IService
{
    public interface IOrderService
    {
        void CreateOrder();
        void UpdateOrder();
        void GetAllOrderById(Guid id);

        List<object> GetAllOrder();

        List<object> GetOrderByUserId(Guid id);
    }
}
