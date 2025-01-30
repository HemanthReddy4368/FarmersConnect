using Application.FarmDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IFarm
    {
        /// <summary>
        /// Creates a new farm for a specific user
        /// </summary>
        /// <param name="userId">The ID of the user who owns the farm</param>
        /// <param name="createFarmDTO">The farm creation data</param>
        /// <returns>Response containing the created farm details</returns>
        Task<FarmResponseDTO> CreateFarmAsync(int userId, CreateFarmDTO createFarmDTO);

        /// <summary>
        /// Updates an existing farm
        /// </summary>
        /// <param name="farmId">The ID of the farm to update</param>
        /// <param name="updateFarmDTO">The updated farm data</param>
        /// <returns>Response containing the updated farm details</returns>
        Task<FarmResponseDTO> UpdateFarmAsync(int farmId, UpdateFarmDTO updateFarmDTO);

        /// <summary>
        /// Retrieves a specific farm by its ID
        /// </summary>
        /// <param name="farmId">The ID of the farm to retrieve</param>
        /// <returns>Response containing the farm details</returns>
        Task<FarmResponseDTO> GetFarmByIdAsync(int farmId);

        /// <summary>
        /// Retrieves all farms belonging to a specific user
        /// </summary>
        /// <param name="userId">The ID of the user whose farms to retrieve</param>
        /// <returns>Response containing a collection of farms</returns>
        Task<FarmsDTO> GetAllFarmsByUserIdAsync(int userId);

        /// <summary>
        /// Retrieves all farms in the system
        /// </summary>
        /// <returns>Response containing a collection of all farms</returns>
        Task<FarmsDTO> GetAllFarmsAsync();

        /// <summary>
        /// Deletes a specific farm
        /// </summary>
        /// <param name="farmId">The ID of the farm to delete</param>
        /// <returns>True if deletion was successful, false otherwise</returns>
        Task<bool> DeleteFarmAsync(int farmId);
    }
}
