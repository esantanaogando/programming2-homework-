using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impresiones3D.Application.DTOs
{
    public class ImpresoraDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Tecnologia { get; set; } = string.Empty; // "FDM","SLA","SLS"
        public string Estado { get; set; } = string.Empty;     // "Disponible","Ocupada","Mantenimiento"
        public int VelocidadMaxima { get; set; }
        public decimal VolumenX { get; set; }
        public decimal VolumenY { get; set; }
        public decimal VolumenZ { get; set; }
    }
}
