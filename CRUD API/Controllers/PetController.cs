using CRUD_API.Data;
using CRUD_API.Models.DTOs;
using CRUD_API.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_API.Controllers
{

    [ApiController]
    [Route("api/[Controller]")]

    public class PetsController : ControllerBase
    {

        private readonly PetDataContext _Pets;

        public PetsController(PetDataContext pets)
        {
            _Pets = pets;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_Pets.Pets.ToList());
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {

            var Pet = _Pets.Pets.Find(id);
            if (Pet == null)
            {
                return NotFound();

            }
            return Ok(Pet);

        }

        [HttpPost]
        public IActionResult Post(PetDTO NewPet)
        {
            var newPet = new Pet
            {

                Specie = NewPet.Specie,
                Race = NewPet.Race,
                Color = NewPet.Color,
                Name = NewPet.Name,
                HumanAge = NewPet.HumanAge
            };

            _Pets.Pets.Add(newPet);
            _Pets.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = newPet.Id }, newPet);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, PetUpdateDTO PetsUp)
        {
            var pet = _Pets.Pets.Find(id);
            if (pet == null)
            {
                return NotFound();
            }
            pet.Name = PetsUp.Name;
            pet.HumanAge = PetsUp.HumanAge;
            _Pets.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var pet = _Pets.Pets.Find(id);

            if (pet == null)
            {
                return NotFound();
            }
            _Pets.Pets.Remove(pet);
            _Pets.SaveChanges();

            return NoContent();
        }



    }
}
