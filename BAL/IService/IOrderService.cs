using DAL.DTO;

namespace BAL.IService
{
    public interface IOrderService
    {
        object CreateOrder(OrderCreateDTO request);
        object UpdateOrder(Guid orderId, OrderUpdateDTO request);
        object GetOrderById(Guid id);
        List<object> GetAllOrder();
        List<object> GetOrderByUserId(Guid id);
        bool CancelOrder(Guid orderId);
    }
}
