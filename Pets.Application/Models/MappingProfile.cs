using CRUD_API.DTOs;
using Pets.Domain.Entities;


namespace CRUD_API.Models
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<Pet, PetDTO>().ReverseMap();

        }

    }
}
