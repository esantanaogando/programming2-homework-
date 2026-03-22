using AutoMapper;
using CRUD_API.DTOs;
using Pets.Application.Contract;
using Pets.Application.Responses;
using Pets.Domain.Entities;
using Pets.Infrastructure.Repositories;

namespace Pets.Application.Services
{
    public class PetService : IPetService
    {
        private readonly UnitOfWork _UnitOfWork;
        private readonly IMapper _mapper;

        public PetService(UnitOfWork uow, IMapper mapper)
        {
            _UnitOfWork = uow;
            _mapper = mapper;
        }

        public ApiResponse<List<PetDTO>> GetAllPets()
        {
            var pets = _UnitOfWork.PetRepository.GetAllPets();
            var data = _mapper.Map<List<PetDTO>>(pets);

            return ApiResponse<List<PetDTO>>.SuccessResponse(
                data,
                "Mascotas obtenidas"
            );
        }

        public ApiResponse<PetDTO> GetPetDetailsById(int id)
        {
            var pet = _UnitOfWork.PetRepository.GetPetById(id);

            if (pet == null)
                throw new Exception("Pet not found");

            var data = _mapper.Map<PetDTO>(pet);

            return ApiResponse<PetDTO>.SuccessResponse(data);
        }

        public ApiResponse<PetDTO> CreatePet(PetDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new Exception("Name is required");

            var pet = _mapper.Map<Pet>(dto);

            _UnitOfWork.PetRepository.AddPet(pet);
            _UnitOfWork.Complete();

            var data = _mapper.Map<PetDTO>(pet);

            return ApiResponse<PetDTO>.SuccessResponse(data, "Creado", 201);
        }

        public ApiResponse<string> UpdatePetInformation(int id, PetUpdateDTO dto)
        {
            var pet = _UnitOfWork.PetRepository.GetPetById(id);

            if (pet == null)
                throw new Exception("Pet not found");

            pet.Name = dto.Name;
            pet.HumanAge = dto.HumanAge;

            _UnitOfWork.PetRepository.UpdatePet(pet);
            _UnitOfWork.Complete();

            return ApiResponse<string>.SuccessResponse(
                "Mascota actualizada correctamente"
            );
        }

        public ApiResponse<string> DeletePet(int id)
        {
            var pet = _UnitOfWork.PetRepository.GetPetById(id);

            if (pet == null)
                throw new Exception("Pet not found");

            _UnitOfWork.PetRepository.DeletePet(id);
            _UnitOfWork.Complete();

            return ApiResponse<string>.SuccessResponse(
                "Mascota eliminada correctamente"
            );
        }
    }
}