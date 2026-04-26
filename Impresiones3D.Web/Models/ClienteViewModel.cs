namespace Impresiones3D.Web.Models
{
    public class ClienteViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? Direccion { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}