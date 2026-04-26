using Impresiones3D.Domain.Enums;
using System.Text.Json.Serialization;

namespace Impresiones3D.Domain.Entities
{
    public class Orden
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        [JsonIgnore]
        public virtual Cliente Cliente { get; set; } = null!;
        public int MaterialId { get; set; }
        [JsonIgnore]
        public virtual Material Material { get; set; } = null!;
        public int? ImpresoraId { get; set; }
        [JsonIgnore]
        public virtual Impresora? Impresora { get; set; }
        public string DescripcionModelo { get; set; } = string.Empty;
        public int CantidadPiezas { get; set; } = 1;
        public double AlturaCapa { get; set; } = 0.2;
        public int Relleno { get; set; } = 20; // porcentaje
        public decimal TiempoEstimadoHoras { get; set; }
        public decimal CostoEstimado { get; set; }
        public EstadoOrden Estado { get; set; } = EstadoOrden.Pendiente;
        public DateTime FechaSolicitud { get; set; } = DateTime.Now;
        public DateTime? FechaInicioProduccion { get; set; }
        public DateTime? FechaFinProduccion { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public string? MotivoCancelacion { get; set; }

        // Sobrecarga de métodos (demostración POO)
        public void CalcularEstimados()
        {
            // Lógica simple: tiempo base * factor de relleno * cantidad
            decimal tiempoBase = 2.0m; // horas por pieza base
            decimal factorRelleno = 1 + (decimal)(Relleno / 100.0);
            TiempoEstimadoHoras = tiempoBase * factorRelleno * CantidadPiezas;
            CostoEstimado = TiempoEstimadoHoras * 5.0m; // $5 por hora
        }

        // Sobrecarga: permite pasar un costo por hora personalizado
        public void CalcularEstimados(decimal costoPorHoraPersonalizado)
        {
            CalcularEstimados(); // calcula tiempo estimado
            CostoEstimado = TiempoEstimadoHoras * costoPorHoraPersonalizado;
        }
    }
}