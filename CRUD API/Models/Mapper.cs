using CRUD_API.DTOs;
using Pets.Domain.Entities;

namespace CRUD_API.Models
{
    public class Mapper: AutoMapper.Profile
    {
        public Mapper() {
            CreateMap<Pet, PetDTO>().ReverseMap();
        
        }

    }
}
