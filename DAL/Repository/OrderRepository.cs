using DAL.DTO;
using DAL.Entities;
using DAL.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ECommerceShoesContext _db;

        public OrderRepository(ECommerceShoesContext db)
        {
            _db = db;
        }

        public object CreateOrder(OrderCreateDTO request)
        {
            if (request?.Items == null || request.Items.Count == 0)
                throw new InvalidOperationException("Order must have at least one item.");

            var user = _db.Users.FirstOrDefault(u => u.UserId == request.UserId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            using var transaction = _db.Database.BeginTransaction();
            try
            {
                double total = 0;
                var orderId = Guid.NewGuid();
                var details = new List<OrderDetail>();

                foreach (var item in request.Items)
                {
                    var product = _db.Products.FirstOrDefault(p => p.ProductId == item.ProductId);
                    if (product == null)
                        throw new InvalidOperationException($"Product {item.ProductId} not found.");
                    if (product.Status == "Inactive")
                        throw new InvalidOperationException($"Product '{product.Name}' is inactive.");
                    if ((product.Quantity ?? 0) < item.Quantity)
                        throw new InvalidOperationException($"Insufficient stock for '{product.Name}'.");

                    var linePrice = (product.Price ?? 0) * item.Quantity;
                    total += linePrice;

                    product.Quantity -= item.Quantity;
                    if (product.Quantity <= 0)
                    {
                        product.Quantity = 0;
                        product.Status = "OutOfStock";
                    }

                    details.Add(new OrderDetail
                    {
                        OrderId = orderId,
                        ProductId = product.ProductId,
                        Quantity = item.Quantity,
                        Price = product.Price
                    });
                }

                var order = new Order
                {
                    OrderId = orderId,
                    UserId = request.UserId,
                    Address = request.Address,
                    TotalAmount = total,
                    CreateAt = DateTime.Now,
                    Status = "Pending"
                };

                _db.Orders.Add(order);
                _db.OrderDetails.AddRange(details);
                _db.SaveChanges();
                transaction.Commit();

                return GetOrderById(orderId);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public List<object> GetAllOrder()
        {
            return _db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .OrderByDescending(o => o.CreateAt)
                .ToList()
                .Select(MapOrder)
                .ToList();
        }

        public object GetOrderById(Guid id)
        {
            var order = _db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.OrderId == id);

            return order == null ? null : MapOrder(order);
        }

        public List<object> GetOrderByUserId(Guid id)
        {
            return _db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.UserId == id)
                .OrderByDescending(o => o.CreateAt)
                .ToList()
                .Select(MapOrder)
                .ToList();
        }

        public object UpdateOrder(Guid orderId, OrderUpdateDTO request)
        {
            var order = _db.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null) return null;

            if (!string.IsNullOrWhiteSpace(request.Status))
                order.Status = request.Status;

            if (!string.IsNullOrWhiteSpace(request.Address))
                order.Address = request.Address;

            _db.SaveChanges();
            return GetOrderById(orderId);
        }

        public bool CancelOrder(Guid orderId)
        {
            var order = _db.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null) return false;
            if (order.Status is "Completed" or "Cancelled" or "Shipping")
                throw new InvalidOperationException($"Cannot cancel order with status '{order.Status}'.");

            using var transaction = _db.Database.BeginTransaction();
            try
            {
                foreach (var detail in order.OrderDetails)
                {
                    var product = _db.Products.FirstOrDefault(p => p.ProductId == detail.ProductId);
                    if (product != null)
                    {
                        product.Quantity = (product.Quantity ?? 0) + (detail.Quantity ?? 0);
                        if (product.Status == "OutOfStock" && product.Quantity > 0)
                            product.Status = "Active";
                    }
                }

                order.Status = "Cancelled";
                _db.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private static object MapOrder(Order o)
        {
            return new
            {
                o.OrderId,
                o.UserId,
                FullName = o.User != null ? o.User.FullName : null,
                o.Address,
                o.CreateAt,
                o.TotalAmount,
                o.Status,
                OrderDetails = o.OrderDetails.Select(od => new
                {
                    od.ProductId,
                    ProductName = od.Product != null ? od.Product.Name : null,
                    od.Quantity,
                    od.Price
                })
            };
        }
    }
}
