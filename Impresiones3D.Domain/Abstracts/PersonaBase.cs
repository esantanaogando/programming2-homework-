namespace Impresiones3D.Domain.Abstracts
{
    public abstract class PersonaBase
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? Direccion { get; set; }

        // Método abstracto que será implementado por Cliente (y futuros Operadores)
        public abstract string ObtenerRol();
    }
}