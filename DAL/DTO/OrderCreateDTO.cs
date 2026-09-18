using System.ComponentModel.DataAnnotations;

namespace DAL.DTO
{
    public class OrderCreateDTO
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [MinLength(1)]
        public List<OrderItemDTO> Items { get; set; } = new();
    }

    public class OrderItemDTO
    {
        [Required]
        public Guid ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
