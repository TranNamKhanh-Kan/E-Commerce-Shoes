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
            if (newUser != null)
            {
                var user = new User();
                user.UserId = Guid.NewGuid();
                user.FullName = newUser.FullName;
                user.Email = newUser.Email;
                user.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
                user.RoleId = 3;
                _db.Users.Add(user);
                _db.SaveChanges();
                return user;
            }
            return null;

        }

        public List<Role> GetRole()
        {
            return _db.Roles.ToList();
        }

        public User GetUser(string email, string password)
        {
            var user = GetUserByEmail(email);
            bool valid = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if (user != null && valid)
            {
                return user;
            }
            return null;
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
            if (user != null)
            {
                user.RoleId = updateUser.RoleId;
                user.FullName = updateUser.FullName;
                user.Email = updateUser.Email;
                user.Password = BCrypt.Net.BCrypt.HashPassword(updateUser.Password);
                user.Phone = updateUser.Phone;
                _db.Users.Update(user);
                _db.SaveChanges();
                return user;
            }
            return null;

        }
    }
}
