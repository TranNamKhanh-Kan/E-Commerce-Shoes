using System.ComponentModel.DataAnnotations;

namespace DAL.DTO
{
    public class UpdateUserDTO
    {
        [Required]
        public Guid UserId { get; set; }

        public int RoleId { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
