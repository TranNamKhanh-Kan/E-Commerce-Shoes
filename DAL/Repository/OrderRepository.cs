using DAL.Entities;
using DAL.IRepository;

namespace DAL.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ECommerceShoesContext _db;

        public OrderRepository(ECommerceShoesContext db)
        {
            _db = db;
        }
        public void CreateOrder()
        {
            throw new NotImplementedException();
        }

        public List<object> GetAllOrder()
        {
            return _db.Orders.Select(o => new
            {
                o.OrderId,
                o.User.FullName,
                o.Address,
                o.CreateAt,
                o.TotalAmount,
                o.Status,
                OrderDetails = o.OrderDetails.Select(od => new
                {
                    od.ProductId,
                    od.Product.Name,
                    od.Quantity,
                    od.Price
                })
                //=====================Cách 2==============
                //o.OrderId,
                //o.User.FullName,
                //orderDetail = o.OrderDetails.Select(od => new
                //{
                //    od.Product.Name,
                //    od.Quantity,
                //    od.Price,
                //})
            }).ToList<object>();
        }

        public void GetAllOrderById(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<object> GetOrderByUserId(Guid id)
        {
            return _db.Orders.Where(o => o.UserId == id).Select(o => new
            {
                o.OrderId,
                o.Status,
                o.CreateAt,
                o.TotalAmount,
                orderDetail = o.OrderDetails.Select(od => new
                {
                    od.Product.Name,
                    od.Quantity,
                    od.Price
                })
            }).ToList<object>();
        }

        public void UpdateOrder()
        {
            throw new NotImplementedException();
        }
    }
}
