namespace DAL.DTO
{
    public class UserResponse
    {
        public Guid UserId { get; set; }
        public int? RoleId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public static UserResponse FromEntity(Entities.User user)
        {
            if (user == null) return null;
            return new UserResponse
            {
                UserId = user.UserId,
                RoleId = user.RoleId,
                FullName = user.FullName,
                Phone = user.Phone,
                Email = user.Email
            };
        }
    }
}
