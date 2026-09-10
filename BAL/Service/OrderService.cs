using BAL.IService;
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
        public void CreateOrder()
        {
            throw new NotImplementedException();
        }

        public List<object> GetAllOrder()
        {
            return _repo.GetAllOrder();
        }

        public void GetAllOrderById(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<object> GetOrderByUserId(Guid id)
        {
            return _repo.GetOrderByUserId(id);
        }

        public void UpdateOrder()
        {
            throw new NotImplementedException();
        }
    }
}
