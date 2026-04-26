using Impresiones3D.API.Responses;
using Impresiones3D.Application.Requests;
using Impresiones3D.Application.Services;
using Impresiones3D.Domain.Entities;
using Impresiones3D.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Impresiones3D.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImpresorasController : ControllerBase
    {
        private readonly ImpresoraService _impresoraService;

        public ImpresorasController(ImpresoraService impresoraService)
        {
            _impresoraService = impresoraService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var impresoras = await _impresoraService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<Impresora>>.Ok(impresoras));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var impresora = await _impresoraService.GetByIdAsync(id);
            if (impresora == null) return NotFound(ApiResponse<object>.Error("Impresora no encontrada"));
            return Ok(ApiResponse<Impresora>.Ok(impresora));
        }

        // NUEVO: asignación automática de la mejor impresora disponible
        // POST api/Impresoras/asignar-automatica?tecnologia=0
        // tecnologia: 0=FDM, 1=SLA, 2=SLS
        [HttpPost("asignar-automatica")]
        public async Task<IActionResult> AsignarAutomatica([FromQuery] TecnologiaImpresora tecnologia)
        {
            try
            {
                var impresora = await _impresoraService.AsignarAutomaticaAsync(tecnologia);
                return Ok(ApiResponse<Impresora>.Ok(impresora,
                    $"Impresora '{impresora.Nombre}' asignada automáticamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Error(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateImpresoraRequest request)
        {
            try
            {
                var impresora = await _impresoraService.CreateAsync(request);
                return Ok(ApiResponse<Impresora>.Ok(impresora, "Impresora registrada"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Error(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateImpresoraRequest request)
        {
            try
            {
                await _impresoraService.UpdateAsync(id, request);
                return Ok(ApiResponse<object>.Ok(null, "Impresora actualizada"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Error(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _impresoraService.DeleteAsync(id);
                return Ok(ApiResponse<object>.Ok(null, "Impresora eliminada"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Error(ex.Message));
            }
        }
    }
}