using DAL.DTO;
using DAL.Entities;
using DAL.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ECommerceShoesContext _db;
        private readonly IOrderRepository _orderRepository;

        public CartRepository(ECommerceShoesContext db, IOrderRepository orderRepository)
        {
            _db = db;
            _orderRepository = orderRepository;
        }

        public object GetCartByUserId(Guid userId)
        {
            var cart = GetOrCreateCart(userId);
            return MapCart(cart.CartId);
        }

        public object AddToCart(AddToCartDTO request)
        {
            var product = _db.Products.FirstOrDefault(p => p.ProductId == request.ProductId);
            if (product == null)
                throw new InvalidOperationException("Product not found.");
            if (product.Status == "Inactive" || product.Status == "OutOfStock")
                throw new InvalidOperationException("Product is not available.");
            if ((product.Quantity ?? 0) < request.Quantity)
                throw new InvalidOperationException("Insufficient stock.");

            var cart = GetOrCreateCart(request.UserId);
            var item = _db.CartItems.FirstOrDefault(ci =>
                ci.CartId == cart.CartId && ci.ProductId == request.ProductId);

            if (item == null)
            {
                _db.CartItems.Add(new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                });
            }
            else
            {
                var newQty = item.Quantity + request.Quantity;
                if ((product.Quantity ?? 0) < newQty)
                    throw new InvalidOperationException("Insufficient stock.");
                item.Quantity = newQty;
            }

            _db.SaveChanges();
            return MapCart(cart.CartId);
        }

        public object UpdateCartItem(UpdateCartItemDTO request)
        {
            var cart = _db.Carts.FirstOrDefault(c => c.UserId == request.UserId);
            if (cart == null)
                throw new InvalidOperationException("Cart not found.");

            var item = _db.CartItems.FirstOrDefault(ci =>
                ci.CartId == cart.CartId && ci.ProductId == request.ProductId);
            if (item == null)
                throw new InvalidOperationException("Cart item not found.");

            var product = _db.Products.FirstOrDefault(p => p.ProductId == request.ProductId);
            if (product == null)
                throw new InvalidOperationException("Product not found.");
            if ((product.Quantity ?? 0) < request.Quantity)
                throw new InvalidOperationException("Insufficient stock.");

            item.Quantity = request.Quantity;
            _db.SaveChanges();
            return MapCart(cart.CartId);
        }

        public bool RemoveCartItem(Guid userId, Guid productId)
        {
            var cart = _db.Carts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null) return false;

            var item = _db.CartItems.FirstOrDefault(ci =>
                ci.CartId == cart.CartId && ci.ProductId == productId);
            if (item == null) return false;

            _db.CartItems.Remove(item);
            _db.SaveChanges();
            return true;
        }

        public bool ClearCart(Guid userId)
        {
            var cart = _db.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserId == userId);
            if (cart == null) return false;

            _db.CartItems.RemoveRange(cart.CartItems);
            _db.SaveChanges();
            return true;
        }

        public object Checkout(CheckoutDTO request)
        {
            var cart = _db.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserId == request.UserId);

            if (cart == null || cart.CartItems.Count == 0)
                throw new InvalidOperationException("Cart is empty.");

            var orderRequest = new OrderCreateDTO
            {
                UserId = request.UserId,
                Address = request.Address,
                Items = cart.CartItems.Select(ci => new OrderItemDTO
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity
                }).ToList()
            };

            var order = _orderRepository.CreateOrder(orderRequest);
            _db.CartItems.RemoveRange(cart.CartItems);
            _db.SaveChanges();
            return order;
        }

        private Cart GetOrCreateCart(Guid userId)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            var cart = _db.Carts.FirstOrDefault(c => c.UserId == userId);
            if (cart != null) return cart;

            cart = new Cart
            {
                CartId = Guid.NewGuid(),
                UserId = userId,
                CreatedAt = DateTime.Now
            };
            _db.Carts.Add(cart);
            _db.SaveChanges();
            return cart;
        }

        private object MapCart(Guid cartId)
        {
            var cart = _db.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefault(c => c.CartId == cartId);

            if (cart == null) return null;

            var items = cart.CartItems.Select(ci => new
            {
                ci.ProductId,
                ProductName = ci.Product?.Name,
                ImageUrl = ci.Product?.ImageUrl,
                UnitPrice = ci.Product?.Price,
                Size = ci.Product?.Size,
                ci.Quantity,
                LineTotal = (ci.Product?.Price ?? 0) * ci.Quantity
            }).ToList();

            return new
            {
                cart.CartId,
                cart.UserId,
                cart.CreatedAt,
                Items = items,
                TotalAmount = items.Sum(i => i.LineTotal)
            };
        }
    }
}
