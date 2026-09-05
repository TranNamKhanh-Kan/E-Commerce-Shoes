using BAL.IService;
using DAL.DTO;
using DAL.Entities;
using DAL.IRepository;

namespace BAL.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;



        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public User CreateNewUser(UserRegisterDTO newUser)
        {
            return _repo.CreateNewUser(newUser);
        }

        public List<User> GetAllUser()
        {
            return _repo.GetUsers();
        }

        public List<Role> GetRoles()
        {
            return _repo.GetRole();
        }

        public User GetUser(string email, string password)
        {
            return _repo.GetUser(email, password);
        }

        public User GetUserByEmail(string email)
        {
            return _repo.GetUserByEmail(email);
        }

        public User GetUserById(Guid userId)
        {
            return _repo.GetUserById(userId);
        }

        public User UpdateNewUser(UpdateUserDTO updateUser)
        {
            return _repo.UpdateNewUser(updateUser);
        }
    }
}
