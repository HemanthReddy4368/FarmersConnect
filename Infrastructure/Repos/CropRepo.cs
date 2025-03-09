// Infrastructure/Repos/CropRepo.cs
using Application.Contracts;
using Application.DTOs;
using Application.DTOs.CropDTOs;
using FarmersConnect.Core.Entites;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repos
{
    public class CropRepo : ICrop
    {
        private readonly AppDbContext _context;

        public CropRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CropResponseDTO> CreateCropAsync(int farmId, CreateCropDTO createCropDTO)
        {
            try
            {
                var crop = new Crop
                {
                    FarmId = farmId,
                    CropName = createCropDTO.CropName,
                    PlantingDate = createCropDTO.PlantingDate,
                    HarvestDate = createCropDTO.HarvestDate,
                    Status = createCropDTO.Status,
                    YieldEstimate = createCropDTO.YieldEstimate
                };

                await _context.Crops.AddAsync(crop);
                await _context.SaveChangesAsync();

                return new CropResponseDTO
                {
                    CropId = crop.CropId,
                    FarmId = crop.FarmId,
                    CropName = crop.CropName,
                    PlantingDate = crop.PlantingDate,
                    HarvestDate = crop.HarvestDate,
                    Status = crop.Status,
                    YieldEstimate = crop.YieldEstimate,
                    flag = true,
                    message = "Crop created successfully"
                };
            }
            catch (Exception ex)
            {
                return new CropResponseDTO
                {
                    flag = false,
                    message = $"Error creating crop: {ex.Message}"
                };
            }
        }

        public async Task<CropResponseDTO> UpdateCropAsync(int cropId, UpdateCropDTO updateCropDTO)
        {
            try
            {
                var crop = await _context.Crops.FindAsync(cropId);
                if (crop == null)
                {
                    return new CropResponseDTO
                    {
                        flag = false,
                        message = "Crop not found"
                    };
                }

                crop.CropName = updateCropDTO.CropName;
                crop.PlantingDate = updateCropDTO.PlantingDate;
                crop.HarvestDate = updateCropDTO.HarvestDate;
                crop.Status = updateCropDTO.Status;
                crop.YieldEstimate = updateCropDTO.YieldEstimate;

                _context.Crops.Update(crop);
                await _context.SaveChangesAsync();

                return new CropResponseDTO
                {
                    CropId = crop.CropId,
                    FarmId = crop.FarmId,
                    CropName = crop.CropName,
                    PlantingDate = crop.PlantingDate,
                    HarvestDate = crop.HarvestDate,
                    Status = crop.Status,
                    YieldEstimate = crop.YieldEstimate,
                    flag = true,
                    message = "Crop updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new CropResponseDTO
                {
                    flag = false,
                    message = $"Error updating crop: {ex.Message}"
                };
            }
        }

        public async Task<CropResponseDTO> GetCropByIdAsync(int cropId)
        {
            try
            {
                var crop = await _context.Crops.FindAsync(cropId);
                if (crop == null)
                {
                    return new CropResponseDTO
                    {
                        flag = false,
                        message = "Crop not found"
                    };
                }

                return new CropResponseDTO
                {
                    CropId = crop.CropId,
                    FarmId = crop.FarmId,
                    CropName = crop.CropName,
                    PlantingDate = crop.PlantingDate,
                    HarvestDate = crop.HarvestDate,
                    Status = crop.Status,
                    YieldEstimate = crop.YieldEstimate,
                    flag = true
                };
            }
            catch (Exception ex)
            {
                return new CropResponseDTO
                {
                    flag = false,
                    message = $"Error retrieving crop: {ex.Message}"
                };
            }
        }

        public async Task<CropsDTO> GetCropsByFarmIdAsync(int farmId)
        {
            try
            {
                var crops = await _context.Crops
                    .Where(c => c.FarmId == farmId)
                    .Select(c => new CropResponseDTO
                    {
                        CropId = c.CropId,
                        FarmId = c.FarmId,
                        CropName = c.CropName,
                        PlantingDate = c.PlantingDate,
                        HarvestDate = c.HarvestDate,
                        Status = c.Status,
                        YieldEstimate = c.YieldEstimate,
                        flag = true
                    })
                    .ToListAsync();

                return new CropsDTO
                {
                    Crops = crops,
                    flag = true,
                    message = "Crops retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new CropsDTO
                {
                    Crops = new List<CropResponseDTO>(),
                    flag = false,
                    message = $"Error retrieving crops: {ex.Message}"
                };
            }
        }

        public async Task<bool> DeleteCropAsync(int cropId)
        {
            try
            {
                var crop = await _context.Crops.FindAsync(cropId);
                if (crop == null)
                    return false;

                _context.Crops.Remove(crop);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<CropResponseDTO> UpdateCropStatusAsync(int cropId, CropStatus newStatus)
        {
            try
            {
                var crop = await _context.Crops.FindAsync(cropId);
                if (crop == null)
                {
                    return new CropResponseDTO
                    {
                        flag = false,
                        message = "Crop not found"
                    };
                }

                crop.Status = newStatus;
                _context.Crops.Update(crop);
                await _context.SaveChangesAsync();

                return new CropResponseDTO
                {
                    CropId = crop.CropId,
                    FarmId = crop.FarmId,
                    CropName = crop.CropName,
                    PlantingDate = crop.PlantingDate,
                    HarvestDate = crop.HarvestDate,
                    Status = crop.Status,
                    YieldEstimate = crop.YieldEstimate,
                    flag = true,
                    message = "Crop status updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new CropResponseDTO
                {
                    flag = false,
                    message = $"Error updating crop status: {ex.Message}"
                };
            }
        }
    }
}