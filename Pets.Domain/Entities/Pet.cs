

namespace Pets.Domain.Entities
{
    public class Pet
    {
        public int Id { get; set; }
       public string Specie {set; get;} = string.Empty;
        public string Race { set; get; } = string.Empty;
        public string Color { set; get;} = string.Empty;
        public string Name { set; get; } = string.Empty;
        public float HumanAge { set; get; }
    }
}
