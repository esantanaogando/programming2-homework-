namespace Impresiones3D.Application.DTOs
{
    public class MaterialMasUsadoDto
    {
        public string MaterialNombre { get; set; } = string.Empty;
        public int TotalUsos { get; set; }
        public decimal CantidadTotalStockUsado { get; set; }
    }
}