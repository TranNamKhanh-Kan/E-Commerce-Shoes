using DAL.DTO;
using DAL.Entities;
using DAL.IRepository;

namespace DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ECommerceShoesContext _db;

        public UserRepository(ECommerceShoesContext db)
        {
            _db = db;
        }

        public User CreateNewUser(UserRegisterDTO newUser)
        {
            if (newUser == null) return null;

            var user = new User
            {
                UserId = Guid.NewGuid(),
                FullName = newUser.FullName,
                Email = newUser.Email,
                Phone = newUser.Phone,
                Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password),
                RoleId = 3
            };
            _db.Users.Add(user);
            _db.SaveChanges();
            return user;
        }

        public List<Role> GetRole()
        {
            return _db.Roles.ToList();
        }

        public User GetUser(string email, string password)
        {
            var user = GetUserByEmail(email);
            if (user == null) return null;

            bool valid = BCrypt.Net.BCrypt.Verify(password, user.Password);
            return valid ? user : null;
        }

        public User GetUserByEmail(string email)
        {
            return _db.Users.FirstOrDefault(u => u.Email == email);
        }

        public User GetUserById(Guid userId)
        {
            return _db.Users.FirstOrDefault(u => u.UserId == userId);
        }

        public List<User> GetUsers()
        {
            return _db.Users.ToList();
        }

        public User UpdateNewUser(UpdateUserDTO updateUser)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserId == updateUser.UserId);
            if (user == null) return null;

            if (updateUser.RoleId > 0)
                user.RoleId = updateUser.RoleId;
            if (!string.IsNullOrWhiteSpace(updateUser.FullName))
                user.FullName = updateUser.FullName;
            if (!string.IsNullOrWhiteSpace(updateUser.Email))
                user.Email = updateUser.Email;
            if (!string.IsNullOrWhiteSpace(updateUser.Phone))
                user.Phone = updateUser.Phone;
            if (!string.IsNullOrWhiteSpace(updateUser.Password))
                user.Password = BCrypt.Net.BCrypt.HashPassword(updateUser.Password);

            _db.SaveChanges();
            return user;
        }
    }
}
