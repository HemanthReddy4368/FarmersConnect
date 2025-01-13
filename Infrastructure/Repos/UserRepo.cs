using Application.Contracts;
using Application.DTOs;
using FarmersConnect.Core.Entites;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    public class UserRepo : IUser
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration configuration;

        public UserRepo(AppDbContext appDbContext, IConfiguration _configuration)
        {
            this._appDbContext = appDbContext;
            this.configuration = _configuration;
        }

        public async Task<LoginResponseDTO> LoginUserAsync(LoginDTO loginDTO)
        {
            var getUser = await _appDbContext.Users.FirstOrDefaultAsync(u=>u.Email == loginDTO.Email);

            if (getUser == null) return new LoginResponseDTO(false, "Please check your username and password");

            bool checkPassowrd = BCrypt.Net.BCrypt.Verify(loginDTO.Password,getUser.Password);

            if (checkPassowrd)
            {
                string token = GenerateJWTToken(getUser);
                return new LoginResponseDTO(true,"Login Successful",token);
            }
            else
            {
                return new LoginResponseDTO(false, "Login Failed check password");
            }
        }

        public async Task<RegestrationResponse> RegesterUserAsync(RegisterUserDTO registerUserDTO)
        {
            var getUser = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == registerUserDTO.Email);
            if (getUser != null)
            {
                return new RegestrationResponse(false, "User Already Exist");
            }
            _appDbContext.Users.Add(new User()
            {
                Name = registerUserDTO.Name,
                Email = registerUserDTO.Email,
                Address = registerUserDTO.Address,
                Role = UserRole.Farmer,
                PhoneNumber = registerUserDTO.Phonenumber,
                Password = BCrypt.Net.BCrypt.HashPassword(registerUserDTO.Password)
            });
            await _appDbContext.SaveChangesAsync();

            return new RegestrationResponse(true,"User Regestered Successfully!");

        }
        private string GenerateJWTToken(User getUser)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, getUser.UserId.ToString()),
                new Claim(ClaimTypes.Name, getUser.Name!),
                new Claim(ClaimTypes.Email, getUser.Email!),
                new Claim(ClaimTypes.Role, getUser.Role.ToString()) // Add role claim
            };

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
