using Impresiones3D.Web.Models;
using Impresiones3D.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Impresiones3D.Web.Controllers
{
    [Authorize]
    public class ImpresorasController : Controller
    {
        private readonly ApiClient _apiClient;

        public ImpresorasController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            var impresoras = await _apiClient.GetAsync<List<ImpresoraViewModel>>("api/Impresoras");
            return View(impresoras ?? new List<ImpresoraViewModel>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var impresora = await _apiClient.GetAsync<ImpresoraViewModel>($"api/Impresoras/{id}");
            if (impresora == null) return NotFound();
            return View(impresora);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ImpresoraViewModel impresora)
        {
            if (!ModelState.IsValid) return View(impresora);

            try
            {
                var request = new
                {
                    impresora.Nombre,
                    impresora.Modelo,
                    impresora.Tecnologia,
                    impresora.VelocidadMaxima,
                    impresora.VolumenX,
                    impresora.VolumenY,
                    impresora.VolumenZ
                };
                var created = await _apiClient.PostAsync<object, ImpresoraViewModel>("api/Impresoras", request);
                if (created != null)
                {
                    TempData["Success"] = "Impresora creada correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error al crear impresora");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(impresora);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var impresora = await _apiClient.GetAsync<ImpresoraViewModel>($"api/Impresoras/{id}");
            if (impresora == null) return NotFound();
            return View(impresora);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ImpresoraViewModel impresora)
        {
            if (id != impresora.Id) return BadRequest();
            if (!ModelState.IsValid) return View(impresora);

            try
            {
                var request = new
                {
                    impresora.Nombre,
                    impresora.Modelo,
                    impresora.Tecnologia,
                    impresora.VelocidadMaxima,
                    impresora.VolumenX,
                    impresora.VolumenY,
                    impresora.VolumenZ
                };
                var success = await _apiClient.PutAsync($"api/Impresoras/{id}", request);
                if (success)
                {
                    TempData["Success"] = "Impresora actualizada correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error al actualizar impresora");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(impresora);
        }

        // GET: Impresoras/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var impresora = await _apiClient.GetAsync<ImpresoraViewModel>($"api/Impresoras/{id}");
            if (impresora == null) return NotFound();
            return View(impresora);
        }

        // POST: Impresoras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _apiClient.DeleteAsync($"api/Impresoras/{id}");
                TempData["Success"] = "Impresora eliminada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Delete), new { id });
            }
        }
    }
}