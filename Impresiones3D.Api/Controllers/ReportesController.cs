using Impresiones3D.API.Responses;
using Impresiones3D.Application.DTOs;
using Impresiones3D.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Impresiones3D.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly ReporteService _reporteService;

        public ReportesController(ReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        // --- Reporte 1: Materiales más usados ---
        [HttpGet("materiales-mas-usados")]
        public async Task<IActionResult> GetMaterialesMasUsados()
        {
            var data = await _reporteService.GetMaterialesMasUsadosAsync();
            return Ok(ApiResponse<IEnumerable<MaterialMasUsadoDto>>.Ok(data));
        }

        [HttpGet("materiales-mas-usados/csv")]
        public async Task<IActionResult> ExportarMaterialesCsv()
        {
            var bytes = await _reporteService.ExportarMaterialesCsvAsync();
            return File(bytes, "text/csv", "materiales_mas_usados.csv");
        }

        // --- Reporte 2: Ingresos proyectados vs reales ---
        // GET api/Reportes/ingresos?desde=2025-01-01&hasta=2025-12-31
        [HttpGet("ingresos")]
        public async Task<IActionResult> GetIngresos(
            [FromQuery] DateTime? desde,
            [FromQuery] DateTime? hasta)
        {
            var data = await _reporteService.GetIngresosAsync(desde, hasta);
            return Ok(ApiResponse<IEnumerable<IngresoReporteDto>>.Ok(data));
        }

        [HttpGet("ingresos/csv")]
        public async Task<IActionResult> ExportarIngresosCsv(
            [FromQuery] DateTime? desde,
            [FromQuery] DateTime? hasta)
        {
            var bytes = await _reporteService.ExportarIngresosCsvAsync(desde, hasta);
            return File(bytes, "text/csv", "reporte_ingresos.csv");
        }

        // --- Reporte 3: Tiempo de uso por impresora ---
        [HttpGet("tiempo-uso-impresoras")]
        public async Task<IActionResult> GetTiempoUsoImpresoras()
        {
            var data = await _reporteService.GetTiempoUsoImpresorasAsync();
            return Ok(ApiResponse<IEnumerable<TiempoUsoImpresoraDto>>.Ok(data));
        }

        [HttpGet("tiempo-uso-impresoras/csv")]
        public async Task<IActionResult> ExportarTiempoUsoImpCsv()
        {
            var bytes = await _reporteService.ExportarTiempoUsoImpresorasCsvAsync();
            return File(bytes, "text/csv", "tiempo_uso_impresoras.csv");
        }
    }
}