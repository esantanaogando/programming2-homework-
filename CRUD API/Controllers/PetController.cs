using CRUD_API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Pets.Application.Contract;
using Pets.Application.Responses;

namespace CRUD_API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class PetsController : ControllerBase
    {
        private readonly IPetService _service;

        public PetsController(IPetService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAllPets()
        {
            try
            {
                var response = _service.GetAllPets();
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message, 400));
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetPetById(int id)
        {
            try
            {
                var response = _service.GetPetDetailsById(id);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return NotFound(ApiResponse<string>.ErrorResponse(ex.Message, 404));
            }
        }

        [HttpPost]
        public IActionResult CreatePet(PetDTO dto)
        {
            try
            {
                var response = _service.CreatePet(dto);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message, 400));
            }
        }

        [HttpPatch("{id}")]
        public IActionResult UpdatePet(int id, PetUpdateDTO dto)
        {
            try
            {
                var response = _service.UpdatePetInformation(id, dto);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return NotFound(ApiResponse<string>.ErrorResponse(ex.Message, 404));
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePet(int id)
        {
            try
            {
                var response = _service.DeletePet(id);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return NotFound(ApiResponse<string>.ErrorResponse(ex.Message, 404));
            }
        }
    }
}