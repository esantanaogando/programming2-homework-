namespace Impresiones3D.Application.DTOs
{
    public class OrdenDto
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public int MaterialId { get; set; }
        public string MaterialNombre { get; set; } = string.Empty;
        public int? ImpresoraId { get; set; }
        public string ImpresoraNombre { get; set; } = string.Empty;
        public string DescripcionModelo { get; set; } = string.Empty;
        public int CantidadPiezas { get; set; }
        public double AlturaCapa { get; set; }
        public int Relleno { get; set; }
        public decimal TiempoEstimadoHoras { get; set; }
        public decimal CostoEstimado { get; set; }
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public string? MotivoCancelacion { get; set; }
    }
}
