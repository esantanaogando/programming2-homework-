using Impresiones3D.Domain.Abstracts;
using System.Text.Json.Serialization;

namespace Impresiones3D.Domain.Entities
{
    public class Cliente : PersonaBase
    {
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [JsonIgnore]
        public virtual ICollection<Orden> Ordenes { get; set; } = new List<Orden>();

        public override string ObtenerRol() => "Cliente";
    }
}