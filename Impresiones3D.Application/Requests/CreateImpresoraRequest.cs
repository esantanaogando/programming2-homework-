using Impresiones3D.Domain.Enums;

namespace Impresiones3D.Application.Requests
{
    public class CreateImpresoraRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public TecnologiaImpresora Tecnologia { get; set; } = TecnologiaImpresora.FDM;
        public int Estado { get; set; }
        public int VelocidadMaxima { get; set; }
        public decimal VolumenX { get; set; }
        public decimal VolumenY { get; set; }
        public decimal VolumenZ { get; set; }
    }
}