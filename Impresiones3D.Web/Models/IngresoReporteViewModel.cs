namespace Impresiones3D.Web.Models
{
    public class IngresoReporteViewModel
    {
        public int OrdenId { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public string MaterialNombre { get; set; } = string.Empty;
        public decimal CostoEstimado { get; set; }
        public decimal CostoReal { get; set; }
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaEntrega { get; set; }
    }
}