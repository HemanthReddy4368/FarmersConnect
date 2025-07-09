using Application.Contracts;
using Application.FarmDTOs;
using FarmersConnect.Core.Entites;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    // Infrastructure/Repos/FarmRepo.cs
    public class FarmRepo : IFarm
    {
        private readonly AppDbContext _context;

        public FarmRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FarmResponseDTO> CreateFarmAsync(int userId, CreateFarmDTO createFarmDTO)
        {
            try
            {
                var farm = new Farm
                {
                    UserId = userId,
                    FarmName = createFarmDTO.FarmName,
                    Location = createFarmDTO.Location,
                    SoilType = createFarmDTO.SoilType,
                    Size = createFarmDTO.Size
                };

                await _context.Farms.AddAsync(farm);
                await _context.SaveChangesAsync();

                return new FarmResponseDTO
                {
                    FarmId = farm.FarmId,
                    UserId = farm.UserId,
                    FarmName = farm.FarmName,
                    Location = farm.Location,
                    SoilType = farm.SoilType,
                    Size = farm.Size,
                    flag = true,
                    message = "Farm created successfully"
                };
            }
            catch (Exception ex)
            {
                return new FarmResponseDTO
                {
                    flag = false,
                    message = $"Error creating farm: {ex.Message}"
                };
            }
        }

        public async Task<FarmResponseDTO> UpdateFarmAsync(int farmId, UpdateFarmDTO updateFarmDTO)
        {
            try
            {
                var farm = await _context.Farms.FindAsync(farmId);
                if (farm == null)
                {
                    return new FarmResponseDTO
                    {
                        flag = false,
                        message = "Farm not found"
                    };
                }

                farm.FarmName = updateFarmDTO.FarmName;
                farm.Location = updateFarmDTO.Location;
                farm.SoilType = updateFarmDTO.SoilType;
                farm.Size = updateFarmDTO.Size;

                _context.Farms.Update(farm);
                await _context.SaveChangesAsync();

                return new FarmResponseDTO
                {
                    FarmId = farm.FarmId,
                    UserId = farm.UserId,
                    FarmName = farm.FarmName,
                    Location = farm.Location,
                    SoilType = farm.SoilType,
                    Size = farm.Size,
                    flag = true,
                    message = "Farm updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new FarmResponseDTO
                {
                    flag = false,
                    message = $"Error updating farm: {ex.Message}"
                };
            }
        }

        public async Task<FarmResponseDTO> GetFarmByIdAsync(int farmId)
        {
            try
            {
                var farm = await _context.Farms.FindAsync(farmId);
                if (farm == null)
                {
                    return new FarmResponseDTO
                    {
                        flag = false,
                        message = "Farm not found"
                    };
                }

                return new FarmResponseDTO
                {
                    FarmId = farm.FarmId,
                    UserId = farm.UserId,
                    FarmName = farm.FarmName,
                    Location = farm.Location,
                    SoilType = farm.SoilType,
                    Size = farm.Size,
                    flag = true
                };
            }
            catch (Exception ex)
            {
                return new FarmResponseDTO
                {
                    flag = false,
                    message = $"Error retrieving farm: {ex.Message}"
                };
            }
        }

        public async Task<FarmsDTO> GetAllFarmsByUserIdAsync(int userId)
        {
            try
            {
                var farms = await _context.Farms
                    .Where(f => f.UserId == userId)
                    .Select(f => new FarmResponseDTO
                    {
                        FarmId = f.FarmId,
                        UserId = f.UserId,
                        FarmName = f.FarmName,
                        Location = f.Location,
                        SoilType = f.SoilType,
                        Size = f.Size,
                        flag = true
                    }).ToListAsync();

                return new FarmsDTO
                {
                    Farms = farms,
                    flag = true,
                    message = "Farms retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new FarmsDTO
                {
                    Farms = new List<FarmResponseDTO>(),
                    flag = false,
                    message = $"Error retrieving farms: {ex.Message}"
                };
            }
        }

        public async Task<FarmsDTO> GetAllFarmsAsync()
        {
            try
            {
                var farms = await _context.Farms
                    .Select(f => new FarmResponseDTO
                    {
                        FarmId = f.FarmId,
                        UserId = f.UserId,
                        FarmName = f.FarmName,
                        Location = f.Location,
                        SoilType = f.SoilType,
                        Size = f.Size,
                        flag = true
                    })
                    .ToListAsync();

                return new FarmsDTO
                {
                    Farms = farms,
                    flag = true,
                    message = "All farms retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new FarmsDTO
                {
                    Farms = new List<FarmResponseDTO>(),
                    flag = false,
                    message = $"Error retrieving farms: {ex.Message}"
                };
            }
        }

        public async Task<bool> DeleteFarmAsync(int farmId)
        {
            try
            {
                var farm = await _context.Farms.FindAsync(farmId);
                if (farm == null)
                    return false;

                _context.Farms.Remove(farm);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
