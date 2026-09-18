using System.ComponentModel.DataAnnotations;

namespace DAL.DTO
{
    public class OrderUpdateDTO
    {
        [Required]
        public string Status { get; set; }

        public string Address { get; set; }
    }
}
