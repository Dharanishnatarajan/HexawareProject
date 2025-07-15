using HotPot.Data;
using HotPot.DTOs;
using HotPot.Interfaces;
using HotPot.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotPot.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly HotPotDbContext _context;

        public AuthService(UserManager<User> userManager,
                          IConfiguration configuration,
                          HotPotDbContext context)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
        }

        public async Task<AuthResponseDTO> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByNameAsync(loginDTO.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDTO.Password))
                throw new UnauthorizedAccessException("Invalid username or password");

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new AuthResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Role = user.Role,
                Username = user.UserName,
                UserId = user.Id
            };
        }

        public async Task<AuthResponseDTO> Register(RegisterDTO registerDTO)
        {
            if (registerDTO.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                throw new ApplicationException("Admin registration is not allowed via API.");

            var userExists = await _userManager.FindByNameAsync(registerDTO.Username);
            if (userExists != null)
                throw new ApplicationException("User already exists!");

            User user = new()
            {
                Email = registerDTO.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerDTO.Username,
                Name = registerDTO.Name,
                Gender = registerDTO.Gender,
                Address = registerDTO.Address,
                Role = registerDTO.Role
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
                throw new ApplicationException("User creation failed! Please check user details and try again.");

            await _userManager.AddToRoleAsync(user, registerDTO.Role);

            // Perform login and return AuthResponseDTO with token etc.
            var loginResponse = await Login(new LoginDTO { Username = registerDTO.Username, Password = registerDTO.Password });

            return loginResponse;
        }




    }
}