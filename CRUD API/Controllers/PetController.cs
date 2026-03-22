using AutoMapper;
using CRUD_API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Pets.Domain.Entities;
using Pets.Infrastructure.Repositories;
using Pets.Persistence;

namespace CRUD_API.Controllers
{

    [ApiController]
    [Route("api/[Controller]")]

    public class PetsController : ControllerBase
    {

        private readonly PetDataContext _Pets;
        private readonly IMapper _Mapper;
        private readonly PetRepository _Petrepository;
        private readonly UnitOfWork _UnitOfWork;

        public PetsController(PetDataContext pets, IMapper mapper, PetRepository petrepository, UnitOfWork unitOfWork)
        {
            _Pets = pets;
            _Mapper = mapper;
            _Petrepository = petrepository;
            _UnitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult<ApiResponse<List<PetDTO>>> GetAll()
        {
            var pets = _Petrepository.GetAllPets();
            var data = _Mapper.Map<List<PetDTO>>(pets);

            var response = ApiResponse<List<PetDTO>>.SuccessResponse(
                data,
                "Mascotas obtenidas"
            );

            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<ApiResponse<PetDTO>> Get(int id)
        {
            var pet = _Petrepository.GetPetById(id);

            if (pet == null)
            {
                return NotFound(ApiResponse<PetDTO>.ErrorResponse("No encontrado", 404));
            }

            var data = _Mapper.Map<PetDTO>(pet);

            var response = ApiResponse<PetDTO>.SuccessResponse(data);

            return Ok(response);
        }

        [HttpPost]
        public ActionResult<ApiResponse<PetDTO>> Post(PetDTO NewPet)
        {
            if (string.IsNullOrWhiteSpace(NewPet.Name))
            {
                return BadRequest(ApiResponse<PetDTO>.ErrorResponse("Name is required", 400));
            }

            var pet = _Mapper.Map<Pet>(NewPet);

            //_Pets.Add(pet);
            //_Pets.SaveChanges();



            var data = _Mapper.Map<PetDTO>(pet);

            var response = ApiResponse<PetDTO>.SuccessResponse(
                data,
                "Mascota creada",
                201
            );

            _UnitOfWork.BeginTransaction();
            _Petrepository.AddPet(pet);
            _UnitOfWork.Complete();
            _UnitOfWork.CommitTransaction();

            try
            {

            }
            catch (Exception)
            {
                _UnitOfWork.RollbackTransaction();

                throw;
            }



            return StatusCode(201, response);
        }

        [HttpPatch("{id}")]
        public ActionResult<ApiResponse<string>> Put(int id, PetUpdateDTO PetsUp)
        {
            var pet = _Petrepository.GetPetById(id);

            if (pet == null)
            {
                return NotFound(ApiResponse<string>.ErrorResponse("No encontrado", 404));
            }

            pet.Name = PetsUp.Name;
            pet.HumanAge = PetsUp.HumanAge;

            _Petrepository.UpdatePet(pet);
            _UnitOfWork.Complete();

            return Ok(ApiResponse<string>.SuccessResponse(
                "Mascota actualizada correctamente"
            ));
        }



        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<string>> Delete(int id)
        {
            var pet = _Petrepository.GetPetById(id);

            if (pet == null)
            {
                return NotFound(ApiResponse<string>.ErrorResponse("No encontrado", 404));
            }

            _Petrepository.DeletePet(id);
            _UnitOfWork.Complete();

            return Ok(ApiResponse<string>.SuccessResponse(
                "Mascota eliminada correctamente"
            ));


        }
    }
}
