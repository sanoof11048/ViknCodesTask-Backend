using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ViknCodesTask.Common;
using ViknCodesTask.Data;
using ViknCodesTask.DTOs.AuthDTOs;
using ViknCodesTask.Interface;
using ViknCodesTask.Models;

namespace ViknCodesTask.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthService(AppDbContext context, IConfiguration config, IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
        }

        public async Task<ApiResponse<AuthResponseDTO>> RegisterAsync(RegisterDTO dto)
        {
            try
            {
                var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
                if (exists)
                    return new ApiResponse<AuthResponseDTO>(400, "Email already registered");

                var user = _mapper.Map<User>(dto);
                user.Id = Guid.NewGuid();
                user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                user.Role = string.IsNullOrEmpty(user.Role) ? "Employee" : user.Role;

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                var token = GenerateJwtToken(user);

                var response = new AuthResponseDTO
                {
                    Token = token,
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = user.Role
                };

                return new ApiResponse<AuthResponseDTO>(200, "User registered successfully", response);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AuthResponseDTO>(500, "Something went wrong", null, ex.Message);
            }
        }

        public async Task<ApiResponse<AuthResponseDTO>> LoginAsync(LoginDTO dto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
                if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                    return new ApiResponse<AuthResponseDTO>(401, "Invalid email or password");

                var token = GenerateJwtToken(user);

                var response = new AuthResponseDTO
                {
                    Token = token,
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = user.Role
                };

                return new ApiResponse<AuthResponseDTO>(200, "Login successful", response);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AuthResponseDTO>(500, "Something went wrong", null, ex.Message);
            }
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
