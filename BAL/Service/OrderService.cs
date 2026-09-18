using BAL.IService;
using DAL.DTO;
using DAL.IRepository;

namespace BAL.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;

        public OrderService(IOrderRepository repo)
        {
            _repo = repo;
        }

        public object CreateOrder(OrderCreateDTO request)
        {
            return _repo.CreateOrder(request);
        }

        public List<object> GetAllOrder()
        {
            return _repo.GetAllOrder();
        }

        public object GetOrderById(Guid id)
        {
            return _repo.GetOrderById(id);
        }

        public List<object> GetOrderByUserId(Guid id)
        {
            return _repo.GetOrderByUserId(id);
        }

        public object UpdateOrder(Guid orderId, OrderUpdateDTO request)
        {
            return _repo.UpdateOrder(orderId, request);
        }

        public bool CancelOrder(Guid orderId)
        {
            return _repo.CancelOrder(orderId);
        }
    }
}
