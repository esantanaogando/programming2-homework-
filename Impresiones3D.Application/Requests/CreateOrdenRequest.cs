namespace Impresiones3D.Application.Requests
{
    public class CreateOrdenRequest
    {
        public int ClienteId { get; set; }
        public int MaterialId { get; set; }
        public string DescripcionModelo { get; set; } = string.Empty;
        public int CantidadPiezas { get; set; } = 1;
        public double AlturaCapa { get; set; } = 0.2;
        public int Relleno { get; set; } = 20;
        public decimal? CostoPorHoraPersonalizado { get; set; } // opcional para sobrecarga
    }
}