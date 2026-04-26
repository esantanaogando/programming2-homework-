using Impresiones3D.Domain.Enums;

namespace Impresiones3D.Domain.Entities
{
    public class Impresora
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public TecnologiaImpresora Tecnologia { get; set; } = TecnologiaImpresora.FDM;
        public EstadoImpresora Estado { get; set; } = EstadoImpresora.Disponible;
        public int VelocidadMaxima { get; set; } // mm/s
        public decimal VolumenX { get; set; }
        public decimal VolumenY { get; set; }
        public decimal VolumenZ { get; set; }

        public decimal HorasTotalesUso { get; set; } = 0;

        public virtual ICollection<Orden> Ordenes { get; set; } = new List<Orden>();
    }
}