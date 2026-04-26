namespace Impresiones3D.Application.DTOs
{
    // Reporte de ingresos proyectados vs reales
    public class IngresoReporteDto
    {
        public int OrdenId { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public string MaterialNombre { get; set; } = string.Empty;
        public decimal CostoEstimado { get; set; }    // ingreso proyectado
        public decimal CostoReal { get; set; }        // 0 si aún no entregada
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaEntrega { get; set; }
    }

    // Reporte de tiempo total de uso por impresora
    public class TiempoUsoImpresoraDto
    {
        public int ImpresoraId { get; set; }
        public string ImpresoraNombre { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Tecnologia { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public decimal HorasTotalesUso { get; set; }
        public int TotalOrdenes { get; set; }
    }
}