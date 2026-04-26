namespace Impresiones3D.Web.Models
{
    public class TiempoUsoImpresoraViewModel
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