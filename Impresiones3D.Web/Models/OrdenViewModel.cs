using System.ComponentModel.DataAnnotations;

namespace Impresiones3D.Web.Models
{
    public class OrdenViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un cliente")]
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un material")]
        public int MaterialId { get; set; }
        public string MaterialNombre { get; set; } = string.Empty;

        public int? ImpresoraId { get; set; }
        public string ImpresoraNombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción del modelo es requerida")]
        public string DescripcionModelo { get; set; } = string.Empty;

        [Range(1, 1000, ErrorMessage = "La cantidad debe ser al menos 1")]
        public int CantidadPiezas { get; set; }

        public double AlturaCapa { get; set; }
        public int Relleno { get; set; }
        public decimal TiempoEstimadoHoras { get; set; }
        public decimal CostoEstimado { get; set; }

        // ⭐ Cambiado de int a string
        public string Estado { get; set; } = string.Empty;

        // Propiedad auxiliar para mostrar el texto del estado (ahora simplemente devuelve Estado)
        public string EstadoTexto => Estado;

        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaInicioProduccion { get; set; }
        public DateTime? FechaFinProduccion { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public string? MotivoCancelacion { get; set; }
    }
}