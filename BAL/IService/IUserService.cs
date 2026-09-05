using DAL.DTO;
using DAL.Entities;

namespace BAL.IService
{
    public interface IUserService
    {
        List<Role> GetRoles();

        List<User> GetAllUser();
        User GetUser(string email, string password);

        User CreateNewUser(UserRegisterDTO newUser);

        User UpdateNewUser(UpdateUserDTO updateUser);

        User GetUserByEmail(string email);
        User GetUserById(Guid userId);
    }
}
