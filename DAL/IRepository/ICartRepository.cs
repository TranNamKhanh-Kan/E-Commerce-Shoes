using DAL.DTO;

namespace DAL.IRepository
{
    public interface ICartRepository
    {
        object GetCartByUserId(Guid userId);
        object AddToCart(AddToCartDTO request);
        object UpdateCartItem(UpdateCartItemDTO request);
        bool RemoveCartItem(Guid userId, Guid productId);
        bool ClearCart(Guid userId);
        object Checkout(CheckoutDTO request);
    }
}
