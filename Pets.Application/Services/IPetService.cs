using CRUD_API.DTOs;
using Pets.Application.Responses;

namespace Pets.Application.Contract
{
    public interface IPetService
    {
        ApiResponse<List<PetDTO>> GetAllPets();

        ApiResponse<PetDTO> GetPetDetailsById(int id);

        ApiResponse<PetDTO> CreatePet(PetDTO dto);

        ApiResponse<string> UpdatePetInformation(int id, PetUpdateDTO dto);

        ApiResponse<string> DeletePet(int id);
    }
}