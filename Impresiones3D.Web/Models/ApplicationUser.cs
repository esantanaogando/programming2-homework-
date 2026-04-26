using Microsoft.AspNetCore.Identity;

namespace Impresiones3D.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? NombreCompleto { get; set; }
    }
}