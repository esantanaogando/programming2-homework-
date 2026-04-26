namespace Impresiones3D.Application.Requests
{
    public class UpdateOrdenRequest
    {
        public int ClienteId { get; set; }
        public int MaterialId { get; set; }
        public int? ImpresoraId { get; set; }   
        public string DescripcionModelo { get; set; } = string.Empty;
        public int CantidadPiezas { get; set; }
        public double AlturaCapa { get; set; }
        public int Relleno { get; set; }
        public decimal? CostoPorHoraPersonalizado { get; set; }
    }
}