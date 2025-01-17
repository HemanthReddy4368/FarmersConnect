using Application.DTOs;
using FarmersConnect.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IUser
    {
        Task<RegestrationResponse> RegesterUserAsync(RegisterUserDTO registerUserDTO);

        Task<LoginResponseDTO> LoginUserAsync(LoginDTO loginDTO);

        Task<UsersDTO> GetAllUsers();

        Task<UserResponseDTO> UpdateUserAsync(int userId, UpdateUserDTO updateUserDTO);
        Task<UserResponseDTO> GetUserByIdAsync(int userId);
        Task<bool> DeleteUserAsync(int userId);
        Task<UserResponseDTO> UpdateUserRoleAsync(int userId, UserRole newRole);
    }
}
