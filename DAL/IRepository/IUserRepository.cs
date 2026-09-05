using DAL.DTO;
using DAL.Entities;

namespace DAL.IRepository
{
    public interface IUserRepository
    {
        List<Role> GetRole();
        List<User> GetUsers();
        User GetUser(string email, string password);

        User CreateNewUser(UserRegisterDTO newUser);

        User UpdateNewUser(UpdateUserDTO updateUser);

        User GetUserByEmail(string email);
        User GetUserById(Guid userId);

    }
}
