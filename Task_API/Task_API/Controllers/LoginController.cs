using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Task_API.Models;
using Task_API.MyModels;
using Task_API.EmailHelper;
namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly TaskManagementContext _context;
        private readonly IConfiguration _configuration;

        public LoginController(TaskManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginInput loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest(new { message = "Vui lòng nhập tài khoản và mật khẩu" });
            }

            var user = _context.Users.FirstOrDefault(u =>
                u.Email == loginRequest.Email &&
                u.PasswordHash == loginRequest.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Sai tài khoản hoặc mật khẩu" });
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role ?? "User")
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return Ok(new
            {
                message = "Đăng nhập thành công",
                token = jwtToken,
                user = new
                {
                    user.UserId,
                    user.Email,
                    user.Role,
                    user.Name,
                    user.PictureUrl,
                }
            });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterInput registerRequest)
        {
            if (!emailCodeMap.ContainsKey(registerRequest.Email))
                return BadRequest("Code not sent.");
            if (registerRequest == null || string.IsNullOrEmpty(registerRequest.Email) ||
                string.IsNullOrEmpty(registerRequest.Password) || string.IsNullOrEmpty(registerRequest.Name))
            {
                return BadRequest(new { message = "Vui lòng nhập đầy đủ thông tin" });
            }

            if (emailCodeMap[registerRequest.Email] != registerRequest.Code)
                return BadRequest(new { message = "Mã xác nhận không đúng" });
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == registerRequest.Email);
            if (existingUser != null)
            {
                return Conflict(new { message = "Email đã được sử dụng" });
            }

            var newUser = new User
            {
                Email = registerRequest.Email,
                PasswordHash = registerRequest.Password, // Nhớ mã hoá nếu làm thật
                Name = registerRequest.Name,
                Role = "User",
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();
            emailCodeMap.Remove(registerRequest.Email);
            return Ok(new { message = "Đăng ký thành công" });
        }

        private static Dictionary<string, string> emailCodeMap = new();
        [HttpPost("sendcode")]
        public async Task<IActionResult> SendCode([FromBody] String email)
        {
            var code = new Random().Next(100000, 999999).ToString();
            await EmailHelper.EmailHelper.SendEmailAsync(email, "WolfTask Daily - Confirmation Code", $"Your code is: {code}");
            emailCodeMap[email] = code;
            return Ok("Code sent successfully.");
        }
        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest(new { message = "Vui lòng nhập email" });

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return NotFound(new { message = "Không tìm thấy người dùng với email này" });

            //string tempPassword = new Random().Next(100000, 999999).ToString();
            //user.PasswordHash = tempPassword;
            //_context.SaveChanges();


            await EmailHelper.EmailHelper.SendEmailAsync(email, "WolfTask", $"Your password is: {user.PasswordHash}");

            return Ok(new { message = "Mật khẩu đã được gửi đến email của bạn" });
        }

    }
}
