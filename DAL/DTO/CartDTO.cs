using System.ComponentModel.DataAnnotations;

namespace DAL.DTO
{
    public class AddToCartDTO
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;
    }

    public class UpdateCartItemDTO
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }

    public class CheckoutDTO
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
