using BAL.IService;
using DAL.DTO;
using DAL.IRepository;

namespace BAL.Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repo;

        public CartService(ICartRepository repo)
        {
            _repo = repo;
        }

        public object GetCartByUserId(Guid userId)
        {
            return _repo.GetCartByUserId(userId);
        }

        public object AddToCart(AddToCartDTO request)
        {
            return _repo.AddToCart(request);
        }

        public object UpdateCartItem(UpdateCartItemDTO request)
        {
            return _repo.UpdateCartItem(request);
        }

        public bool RemoveCartItem(Guid userId, Guid productId)
        {
            return _repo.RemoveCartItem(userId, productId);
        }

        public bool ClearCart(Guid userId)
        {
            return _repo.ClearCart(userId);
        }

        public object Checkout(CheckoutDTO request)
        {
            return _repo.Checkout(request);
        }
    }
}
