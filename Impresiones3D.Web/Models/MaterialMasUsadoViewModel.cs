namespace Impresiones3D.Web.Models
{
    public class MaterialMasUsadoViewModel
    {
        public string MaterialNombre { get; set; } = string.Empty;
        public int TotalUsos { get; set; }
        public decimal CantidadTotalStockUsado { get; set; }
    }
}
