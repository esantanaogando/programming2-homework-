namespace Impresiones3D.Application.Requests
{
    public class CreateMaterialRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public decimal PrecioPorGramo { get; set; }
        public decimal Stock { get; set; }
        public string? UnidadMedida { get; set; }
    }
}