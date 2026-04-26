namespace Impresiones3D.Domain.Entities
{
    public class Material
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public decimal PrecioPorGramo { get; set; }
        public decimal Stock { get; set; }
        public string UnidadMedida { get; set; } = "gramos";
        public virtual ICollection<Orden> Ordenes { get; set; } = new List<Orden>();
    }
}