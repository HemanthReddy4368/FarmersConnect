using Application.DTOs.CropDTOs;
using FarmersConnect.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface ICrop
    {
        Task<CropResponseDTO> CreateCropAsync(int farmId, CreateCropDTO createCropDTO);
        Task<CropResponseDTO> UpdateCropAsync(int cropId, UpdateCropDTO updateCropDTO);
        Task<CropResponseDTO> GetCropByIdAsync(int cropId);
        Task<CropsDTO> GetCropsByFarmIdAsync(int farmId);
        Task<bool> DeleteCropAsync(int cropId);
        Task<CropResponseDTO> UpdateCropStatusAsync(int cropId, CropStatus newStatus);
    }
}
