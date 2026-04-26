namespace Impresiones3D.Web.Models
{
    public class ImpresoraViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int Tecnologia { get; set; } // 1=FDM, 2=SLA, 3=SLS
        public string TecnologiaTexto => Tecnologia switch { 1 => "FDM", 2 => "SLA", 3 => "SLS", _ => "Desconocida" };
        public int Estado { get; set; }
        public string EstadoTexto => Estado switch { 1 => "Disponible", 2 => "Ocupada", 3 => "Mantenimiento", _ => "Desconocido" };
        public int VelocidadMaxima { get; set; }
        public decimal VolumenX { get; set; }
        public decimal VolumenY { get; set; }
        public decimal VolumenZ { get; set; }
    }
}