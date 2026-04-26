using Impresiones3D.Web.Models;
using Impresiones3D.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Impresiones3D.Web.Controllers
{
    public class OrdenesController : Controller
    {
        private readonly ApiClient _apiClient;

        public OrdenesController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            var ordenes = await _apiClient.GetAsync<List<OrdenViewModel>>("api/Ordenes");
            return View(ordenes ?? new List<OrdenViewModel>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var orden = await _apiClient.GetAsync<OrdenViewModel>($"api/Ordenes/{id}");
            if (orden == null) return NotFound();
            return View(orden);
        }

        public async Task<IActionResult> Create()
        {
            await CargarListasDesplegables();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrdenViewModel orden)
        {
            if (!ModelState.IsValid)
            {
                await CargarListasDesplegables(orden.ClienteId, orden.MaterialId, orden.ImpresoraId);
                return View(orden);
            }

            var request = new
            {
                orden.ClienteId,
                orden.MaterialId,
                orden.DescripcionModelo,
                orden.CantidadPiezas,
                orden.AlturaCapa,
                orden.Relleno,
                CostoPorHoraPersonalizado = (decimal?)null
            };

            try
            {
                var created = await _apiClient.PostAsync<object, OrdenViewModel>("api/Ordenes", request);
                if (created != null) return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", "Error inesperado al crear la orden.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            await CargarListasDesplegables(orden.ClienteId, orden.MaterialId, orden.ImpresoraId);
            return View(orden);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var orden = await _apiClient.GetAsync<OrdenViewModel>($"api/Ordenes/{id}");
            if (orden == null) return NotFound();

            // Verificar si la orden está entregada o cancelada
            if (orden.Estado == "Entregada" || orden.Estado == "Cancelada")
            {
                TempData["Error"] = "No se puede editar una orden que ya está entregada o cancelada.";
                return RedirectToAction(nameof(Index));
            }

            await CargarListasDesplegables(orden.ClienteId, orden.MaterialId, orden.ImpresoraId);
            return View(orden);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrdenViewModel orden)
        {
            if (id != orden.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                await CargarListasDesplegables(orden.ClienteId, orden.MaterialId, orden.ImpresoraId);
                return View(orden);
            }

            var request = new
            {
                orden.ClienteId,
                orden.MaterialId,
                orden.ImpresoraId,
                orden.DescripcionModelo,
                orden.CantidadPiezas,
                orden.AlturaCapa,
                orden.Relleno,
                CostoPorHoraPersonalizado = (decimal?)null
            };

            try
            {
                var success = await _apiClient.PutAsync($"api/Ordenes/{id}", request);
                if (success) return RedirectToAction(nameof(Index));
                // Si llegamos aquí, algo falló sin excepción
                ModelState.AddModelError("", "No se pudo actualizar la orden.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            await CargarListasDesplegables(orden.ClienteId, orden.MaterialId, orden.ImpresoraId);
            return View(orden);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var orden = await _apiClient.GetAsync<OrdenViewModel>($"api/Ordenes/{id}");
            if (orden == null) return NotFound();
            return View(orden);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                await _apiClient.DeleteAsync($"api/Ordenes/{id}");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var orden = await _apiClient.GetAsync<OrdenViewModel>($"api/Ordenes/{id}");
                if (orden == null) return NotFound();
                return View("Delete", orden);
            }
        }

        public async Task<IActionResult> CambiarEstado(int id)
        {
            var orden = await _apiClient.GetAsync<OrdenViewModel>($"api/Ordenes/{id}");
            if (orden == null) return NotFound();

            ViewBag.Estados = new SelectList(new[]
            {
                "Pendiente", "EnProduccion", "Pausada", "Completada", "Entregada", "Cancelada"
            });

            var impresoras = await _apiClient.GetAsync<List<ImpresoraViewModel>>("api/Impresoras");
            ViewBag.Impresoras = new SelectList(impresoras, "Id", "Nombre");

            return View(orden);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id, string nuevoEstado, string? motivo, int? impresoraId)
        {
            int estadoInt = nuevoEstado switch
            {
                "Pendiente" => 1,
                "EnProduccion" => 2,
                "Pausada" => 3,
                "Completada" => 4,
                "Entregada" => 5,
                "Cancelada" => 6,
                _ => 1
            };

            try
            {
                object queryParams;
                if (impresoraId.HasValue)
                    queryParams = new { nuevoEstado = estadoInt, motivo = motivo ?? "", impresoraId = impresoraId.Value };
                else
                    queryParams = new { nuevoEstado = estadoInt, motivo = motivo ?? "" };

                await _apiClient.PatchAsync($"api/Ordenes/{id}/estado", queryParams);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var orden = await _apiClient.GetAsync<OrdenViewModel>($"api/Ordenes/{id}");
                if (orden == null) return NotFound();
                ViewBag.Estados = new SelectList(new[]
                {
                    "Pendiente", "EnProduccion", "Pausada", "Completada", "Entregada", "Cancelada"
                });
                var impresoras = await _apiClient.GetAsync<List<ImpresoraViewModel>>("api/Impresoras");
                ViewBag.Impresoras = new SelectList(impresoras, "Id", "Nombre");
                return View(orden);
            }
        }

        public async Task<IActionResult> DescargarComprobante(int id)
        {
            var response = await _apiClient.GetByteArrayAsync($"api/Ordenes/{id}/comprobante");
            if (response == null) return NotFound();
            return File(response, "text/plain", $"Comprobante_{id}.txt");
        }

        private async Task CargarListasDesplegables(int? clienteId = null, int? materialId = null, int? impresoraId = null)
        {
            var clientes = await _apiClient.GetAsync<List<ClienteViewModel>>("api/Clientes");
            ViewBag.Clientes = new SelectList(clientes, "Id", "Nombre", clienteId);

            var materiales = await _apiClient.GetAsync<List<MaterialViewModel>>("api/Materiales");
            ViewBag.Materiales = new SelectList(materiales, "Id", "Nombre", materialId);

            var impresoras = await _apiClient.GetAsync<List<ImpresoraViewModel>>("api/Impresoras");
            ViewBag.Impresoras = new SelectList(impresoras, "Id", "Nombre", impresoraId);
        }
    }
}