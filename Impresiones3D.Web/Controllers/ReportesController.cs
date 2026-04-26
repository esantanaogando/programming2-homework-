using Impresiones3D.Web.Models;
using Impresiones3D.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Impresiones3D.Web.Controllers
{
    public class ReportesController : Controller
    {
        private readonly ApiClient _apiClient;

        public ReportesController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> MaterialesMasUsados()
        {
            var data = await _apiClient.GetAsync<List<MaterialMasUsadoViewModel>>("api/Reportes/materiales-mas-usados");
            return View(data ?? new List<MaterialMasUsadoViewModel>());
        }

        public async Task<IActionResult> Ingresos(DateTime? desde, DateTime? hasta)
        {
            var url = "api/Reportes/ingresos";
            var parametros = new List<string>();
            if (desde.HasValue) parametros.Add($"desde={desde.Value:yyyy-MM-dd}");
            if (hasta.HasValue) parametros.Add($"hasta={hasta.Value:yyyy-MM-dd}");
            if (parametros.Any()) url += "?" + string.Join("&", parametros);

            var data = await _apiClient.GetAsync<List<IngresoReporteViewModel>>(url);
            ViewBag.Desde = desde;
            ViewBag.Hasta = hasta;
            return View(data ?? new List<IngresoReporteViewModel>());
        }

        public async Task<IActionResult> TiempoUsoImpresoras()
        {
            var data = await _apiClient.GetAsync<List<TiempoUsoImpresoraViewModel>>("api/Reportes/tiempo-uso-impresoras");
            return View(data ?? new List<TiempoUsoImpresoraViewModel>());
        }

        [HttpPost]
        public async Task<IActionResult> ExportarIngresosCsv(DateTime? desde, DateTime? hasta)
        {
            var url = "api/Reportes/ingresos/csv";
            var parametros = new List<string>();
            if (desde.HasValue) parametros.Add($"desde={desde.Value:yyyy-MM-dd}");
            if (hasta.HasValue) parametros.Add($"hasta={hasta.Value:yyyy-MM-dd}");
            if (parametros.Any()) url += "?" + string.Join("&", parametros);

            var bytes = await _apiClient.GetByteArrayAsync(url);
            if (bytes == null) return NotFound();
            return File(bytes, "text/csv", $"ingresos_{DateTime.Now:yyyyMMdd}.csv");
        }

        [HttpPost]
        public async Task<IActionResult> ExportarUsoImpresorasCsv()
        {
            var bytes = await _apiClient.GetByteArrayAsync("api/Reportes/tiempo-uso-impresoras/csv");
            if (bytes == null) return NotFound();
            return File(bytes, "text/csv", $"uso_impresoras_{DateTime.Now:yyyyMMdd}.csv");
        }
    }
}