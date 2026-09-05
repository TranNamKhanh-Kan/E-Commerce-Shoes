using BAL.IService;
using DAL.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace API_E_Commerce_Shoes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJWTService _jwtService;

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            try
            {
                var mailAddress = new MailAddress(email);

                return mailAddress.Address == email.Trim();
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                return false;

            bool hasUpperCase = password.Any(char.IsUpper);
            bool hasDigit = password.Any(char.IsDigit);

            return hasUpperCase && hasDigit;
        }

        public UserController(IUserService userService, IJWTService jWTService)
        {
            _userService = userService;
            _jwtService = jWTService;
        }
        [HttpGet("role")]
        public IActionResult GetRoles()
        {
            return Ok(_userService.GetRoles());
        }

        [HttpPost("register")]
        public IActionResult Register(UserRegisterDTO registerDTO)
        {

            if (!IsValidEmail(registerDTO.Email))
            {
                return BadRequest("Invalid mail");
            }
            else if (_userService.GetUserByEmail(registerDTO.Email) != null)
            {
                return BadRequest("Email Exited");
            }
            else if (!IsValidPassword(registerDTO.Password))
            {
                return BadRequest("Invalid password");
            }
            return Ok(_userService.CreateNewUser(registerDTO));
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO login)
        {
            var user = _userService.GetUser(login.email, login.password);
            if (user != null)
            {
                return Ok(new { user, token = _jwtService.GenerateToken(user.Email, user.RoleId) });
            }
            return BadRequest("Login fail");
        }
        [HttpPut("update-user")]
        [Authorize]
        public IActionResult UpdateUser(UpdateUserDTO updateUser)
        {
            return Ok(_userService.UpdateNewUser(updateUser));
        }

        [HttpGet("get-user-by-email")]
        [Authorize]
        public IActionResult GetUserByEmail([FromBody] string email)
        {
            return Ok(_userService.GetUserByEmail(email));
        }
        [HttpGet("get-all-user")]
        [Authorize(Roles = "1")]
        public IActionResult GetAllUser()
        {
            return Ok(_userService.GetAllUser());
        }
    }
}
