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
            var getUser = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

            if (getUser == null) return new LoginResponseDTO(false, "Please check your username and password");

            bool checkPassowrd = BCrypt.Net.BCrypt.Verify(loginDTO.Password, getUser.Password);

            if (checkPassowrd)
            {
                string token = GenerateJWTToken(getUser);
                return new LoginResponseDTO(true, "Login Successful", token);
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

            return new RegestrationResponse(true, "User Regestered Successfully!");

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

        public async Task<UserResponseDTO> GetUserByIdAsync(int userId)
        {
            var user = await _appDbContext.Users.FindAsync(userId);
            if (user == null)
                return new UserResponseDTO(null, false, "User not found");
            var userdata = new UserDataDTO
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Role = user.Role
            };
            return new UserResponseDTO(userdata, true, "User found");
        }

        public async Task<UserResponseDTO> UpdateUserAsync(int userId, UpdateUserDTO updateUserDTO)
        {
            var user = await _appDbContext.Users.FindAsync(userId);
            if (user == null)
                return new UserResponseDTO(null, false, "User not found");

            // Update only provided fields
            if (!string.IsNullOrEmpty(updateUserDTO.Name))
                user.Name = updateUserDTO.Name;
            if (!string.IsNullOrEmpty(updateUserDTO.Email))
                user.Email = updateUserDTO.Email;
            if (!string.IsNullOrEmpty(updateUserDTO.PhoneNumber))
                user.PhoneNumber = updateUserDTO.PhoneNumber;
            if (!string.IsNullOrEmpty(updateUserDTO.Address))
                user.Address = updateUserDTO.Address;
            user.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _appDbContext.SaveChangesAsync();
                var userdata = new UserDataDTO
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    Role = user.Role
                };
                return new UserResponseDTO(userdata, true, "User updated successfully");
            }
            catch (Exception ex)
            {
                return new UserResponseDTO(null, false, $"Error updating user: {ex.Message}");
            }
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _appDbContext.Users.FindAsync(userId);
            if (user == null)
                return false;

            // Soft delete implementation
            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;

            try
            {
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<UserResponseDTO> UpdateUserRoleAsync(int userId, UserRole newRole)
        {
            var user = await _appDbContext.Users.FindAsync(userId);
            if (user == null)
                return new UserResponseDTO(null, false, "User not found");

            user.Role = newRole;

            try
            {
                await _appDbContext.SaveChangesAsync();
                var userdata = new UserDataDTO
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    Role = user.Role
                };
                return new UserResponseDTO(userdata, true, "User role updated successfully");
            }
            catch (Exception ex)
            {
                return new UserResponseDTO(null, false, $"Error updating user role: {ex.Message}");
            }
        }

        public async Task<UsersDTO> GetAllUsers()
        {
            var users = await _appDbContext.Users.ToListAsync();

            if (users.Count == 0)
                return new UsersDTO(null, false, "No users found");

            var userDTOs = users.Select(user => new UserDataDTO
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Role = user.Role
            }).ToList();
            return new UsersDTO(userDTOs, true, "");
        }
    }
}
