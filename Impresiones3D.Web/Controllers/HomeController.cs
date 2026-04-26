using Impresiones3D.Web.Models;
using Impresiones3D.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Impresiones3D.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiClient _apiClient;

        public HomeController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            var dashboard = new DashboardViewModel();

            try
            {
                var clientes = await _apiClient.GetAsync<List<ClienteViewModel>>("api/Clientes");
                dashboard.TotalClientes = clientes?.Count ?? 0;

                var materiales = await _apiClient.GetAsync<List<MaterialViewModel>>("api/Materiales");
                dashboard.TotalMateriales = materiales?.Count ?? 0;

                var impresoras = await _apiClient.GetAsync<List<ImpresoraViewModel>>("api/Impresoras");
                dashboard.TotalImpresoras = impresoras?.Count ?? 0;

                var ordenes = await _apiClient.GetAsync<List<OrdenViewModel>>("api/Ordenes");
                dashboard.TotalOrdenes = ordenes?.Count ?? 0;

                if (ordenes != null)
                {
                    dashboard.OrdenesPendientes = ordenes.Count(o => o.Estado == "Pendiente");
                    dashboard.OrdenesEnProduccion = ordenes.Count(o => o.Estado == "EnProduccion");
                    dashboard.OrdenesCompletadas = ordenes.Count(o => o.Estado == "Completada");
                    dashboard.OrdenesEntregadas = ordenes.Count(o => o.Estado == "Entregada");
                    dashboard.OrdenesCanceladas = ordenes.Count(o => o.Estado == "Cancelada");
                    dashboard.OrdenesRecientes = ordenes.OrderByDescending(o => o.FechaSolicitud).Take(5).ToList();
                }
            }
            catch (Exception ex)
            {
                // En caso de error, mostramos valores por defecto y logueamos
                Console.WriteLine($"Error cargando dashboard: {ex.Message}");
            }

            return View(dashboard);
        }
    }
}