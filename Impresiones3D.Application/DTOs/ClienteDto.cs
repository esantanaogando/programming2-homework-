using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impresiones3D.Application.DTOs
{
    public class ClienteDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? Direccion { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
