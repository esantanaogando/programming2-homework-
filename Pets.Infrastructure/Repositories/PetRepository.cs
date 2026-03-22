using Pets.Domain.Entities;
using Pets.Persistence;

namespace Pets.Infrastructure.Repositories
{

    public class PetRepository
    {
        private readonly PetDataContext _Pets;

        public PetRepository(PetDataContext pets)
        {
            _Pets = pets;
        }

        public List<Pet> GetAllPets()
        {
            return _Pets.Pets.ToList();
        }

        public Pet GetPetById(int id)
        {
            return _Pets.Pets.FirstOrDefault(p => p.Id == id);
        }

        public void AddPet(Pet pet)
        {
            _Pets.Pets.Add(pet);
            // _Pets.SaveChanges();
        }

        public void UpdatePet(Pet pet)
        {
            var existingPet = _Pets.Pets.FirstOrDefault(p => p.Id == pet.Id);
            if (existingPet != null)
            {
                existingPet.Name = pet.Name;
                existingPet.HumanAge = pet.HumanAge;
                // _Pets.SaveChanges();
            }
        }

        public void DeletePet(int id)
        {
            var pet = _Pets.Pets.Find(id);
            if (pet != null)
            {
                _Pets.Pets.Remove(pet);
                // _Pets.SaveChanges();
            }

        }
    }
}
