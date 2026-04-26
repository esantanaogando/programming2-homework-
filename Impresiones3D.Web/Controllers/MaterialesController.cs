using Impresiones3D.Web.Models;
using Impresiones3D.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Impresiones3D.Web.Controllers
{
    [Authorize]
    public class MaterialesController : Controller
    {
        private readonly ApiClient _apiClient;

        public MaterialesController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            var materiales = await _apiClient.GetAsync<List<MaterialViewModel>>("api/Materiales");
            return View(materiales ?? new List<MaterialViewModel>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var material = await _apiClient.GetAsync<MaterialViewModel>($"api/Materiales/{id}");
            if (material == null) return NotFound();
            return View(material);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaterialViewModel material)
        {
            if (!ModelState.IsValid) return View(material);

            try
            {
                var request = new
                {
                    material.Nombre,
                    material.Color,
                    material.PrecioPorGramo,
                    material.Stock,
                    material.UnidadMedida
                };
                var created = await _apiClient.PostAsync<object, MaterialViewModel>("api/Materiales", request);
                if (created != null)
                {
                    TempData["Success"] = "Material creado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error al crear material");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(material);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var material = await _apiClient.GetAsync<MaterialViewModel>($"api/Materiales/{id}");
            if (material == null) return NotFound();
            return View(material);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MaterialViewModel material)
        {
            if (id != material.Id) return BadRequest();
            if (!ModelState.IsValid) return View(material);

            try
            {
                var request = new
                {
                    material.Nombre,
                    material.Color,
                    material.PrecioPorGramo,
                    material.Stock,
                    material.UnidadMedida
                };
                var success = await _apiClient.PutAsync($"api/Materiales/{id}", request);
                if (success)
                {
                    TempData["Success"] = "Material actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error al actualizar material");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(material);
        }

        // GET: Materiales/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var material = await _apiClient.GetAsync<MaterialViewModel>($"api/Materiales/{id}");
            if (material == null) return NotFound();
            return View(material);
        }

        // POST: Materiales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _apiClient.DeleteAsync($"api/Materiales/{id}");
                TempData["Success"] = "Material eliminado correctamente.";
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