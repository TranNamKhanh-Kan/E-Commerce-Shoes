using System.ComponentModel.DataAnnotations;

namespace DAL.DTO
{
    public class UserRegisterDTO
    {
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Phone { get; set; }
    }
}
