using Impresiones3D.API.Responses;
using Impresiones3D.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Impresiones3D.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfiguracionesController : ControllerBase
    {
        private readonly ConfiguracionService _configService;

        public ConfiguracionesController(ConfiguracionService configService)
        {
            _configService = configService;
        }

        [HttpGet("{clave}")]
        public async Task<IActionResult> Get(string clave)
        {
            var valor = await _configService.ObtenerValor(clave);
            if (valor == null) return NotFound(ApiResponse<object>.Error("Clave no encontrada"));
            return Ok(ApiResponse<string>.Ok(valor));
        }

        [HttpPut("{clave}")]
        public async Task<IActionResult> Update(string clave, [FromBody] string nuevoValor)
        {
            await _configService.ActualizarConfiguracion(clave, nuevoValor);
            return Ok(ApiResponse<object>.Ok(null, "Configuración actualizada"));
        }
    }
}