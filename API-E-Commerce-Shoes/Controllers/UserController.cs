using BAL.IService;
using DAL.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Security.Claims;

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
                return BadRequest("Invalid mail");
            if (_userService.GetUserByEmail(registerDTO.Email) != null)
                return BadRequest("Email Existed");
            if (!IsValidPassword(registerDTO.Password))
                return BadRequest("Invalid password");

            var user = _userService.CreateNewUser(registerDTO);
            return Ok(UserResponse.FromEntity(user));
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO login)
        {
            var user = _userService.GetUser(login.email, login.password);
            if (user == null)
                return BadRequest("Login fail");

            return Ok(new
            {
                user = UserResponse.FromEntity(user),
                token = _jwtService.GenerateToken(user.Email, user.RoleId)
            });
        }

        [HttpPut("update-user")]
        [Authorize]
        public IActionResult UpdateUser(UpdateUserDTO updateUser)
        {
            var user = _userService.UpdateNewUser(updateUser);
            if (user == null) return NotFound("User not found");
            return Ok(UserResponse.FromEntity(user));
        }

        [HttpGet("get-user-by-email")]
        [Authorize]
        public IActionResult GetUserByEmail([FromQuery] string email)
        {
            var user = _userService.GetUserByEmail(email);
            if (user == null) return NotFound("User not found");
            return Ok(UserResponse.FromEntity(user));
        }

        [HttpGet("get-user-by-id/{id:guid}")]
        [Authorize]
        public IActionResult GetUserById(Guid id)
        {
            var user = _userService.GetUserById(id);
            if (user == null) return NotFound("User not found");
            return Ok(UserResponse.FromEntity(user));
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")
                ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var user = _userService.GetUserByEmail(email);
            if (user == null) return NotFound("User not found");
            return Ok(UserResponse.FromEntity(user));
        }

        [HttpGet("get-all-user")]
        [Authorize(Roles = "1")]
        public IActionResult GetAllUser()
        {
            var users = _userService.GetAllUser()
                .Select(UserResponse.FromEntity)
                .ToList();
            return Ok(users);
        }
    }
}
