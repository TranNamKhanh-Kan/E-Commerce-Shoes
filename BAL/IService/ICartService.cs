using DAL.DTO;

namespace BAL.IService
{
    public interface ICartService
    {
        object GetCartByUserId(Guid userId);
        object AddToCart(AddToCartDTO request);
        object UpdateCartItem(UpdateCartItemDTO request);
        bool RemoveCartItem(Guid userId, Guid productId);
        bool ClearCart(Guid userId);
        object Checkout(CheckoutDTO request);
    }
}
