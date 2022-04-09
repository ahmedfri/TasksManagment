using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Entities.Models;
using Entities.DataTransferObjects.Authentication;
using Entities.DataTransferObjects.Commons;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace TasksManagmentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicationUserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public ApplicationUserController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this._userManager = userManager;
            _configuration = configuration;
        }
        [HttpPost]
        [Route(ApiRoute.ApplicatonUserRoutes.Register)]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var userExists = await _userManager.FindByNameAsync(registerDto.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { StatusCode = 400, ResponseMessage = "User already exists!" });
            ApplicationUser user = new()
            {
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerDto.Username,
                PhoneNumber = registerDto.PhoneNumer,
                UserType = registerDto.UserType
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { StatusCode = 400, ResponseMessage = "User creation failed! Please check user details and try again." });
            return Ok(new ResponseDto { StatusCode = 200, ResponseMessage = "User created successfully!" });
        }

        [HttpGet]
        [Route(ApiRoute.ApplicatonUserRoutes.GetAllEmployees)]
        public async Task<IActionResult> GetAllEmployees()
        {
            var users = await _userManager.Users.ToListAsync();
            try
            {
                return Ok(users);
            }
            catch (Exception ex)
            {
                BaseResponse response = new() { ResponseMessage = ex.Message, StatusCode = 401 };
                return BadRequest(response);
            }
        }
        [HttpPost]
        [Route(ApiRoute.ApplicatonUserRoutes.Login)]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("UserType", user.UserType.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, user.Id),
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
                var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMonths(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    Usertype = user.UserType
                });
            }
            return Unauthorized();
        }
    }
}
