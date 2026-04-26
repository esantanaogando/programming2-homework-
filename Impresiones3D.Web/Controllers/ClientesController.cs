using Impresiones3D.Web.Models;
using Impresiones3D.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Impresiones3D.Web.Controllers
{
    [Authorize] // Todo el controlador requiere autenticación
    public class ClientesController : Controller
    {
        private readonly ApiClient _apiClient;

        public ClientesController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        // GET: /Clientes
        public async Task<IActionResult> Index()
        {
            try
            {
                var clientes = await _apiClient.GetAsync<List<ClienteViewModel>>("api/Clientes");
                return View(clientes ?? new List<ClienteViewModel>());
            }
            catch (Exception ex)
            {
                // Muestra el error en la vista o en la consola de depuración
                Console.WriteLine(ex.Message);
                ModelState.AddModelError("", "Error al cargar clientes: " + ex.Message);
                return View(new List<ClienteViewModel>());
            }
        }

        // GET: /Clientes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var cliente = await _apiClient.GetAsync<ClienteViewModel>($"api/Clientes/{id}");
            if (cliente == null)
                return NotFound();
            return View(cliente);
        }

        // GET: /Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteViewModel cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

            var request = new
            {
                cliente.Nombre,
                cliente.Correo,
                cliente.Telefono,
                cliente.Direccion
            };
            var created = await _apiClient.PostAsync<object, ClienteViewModel>("api/Clientes", request);
            if (created != null)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Error al crear cliente");
            return View(cliente);
        }

        // GET: /Clientes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _apiClient.GetAsync<ClienteViewModel>($"api/Clientes/{id}");
            if (cliente == null)
                return NotFound();
            return View(cliente);
        }

        // POST: /Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClienteViewModel cliente)
        {
            if (id != cliente.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(cliente);

            var request = new
            {
                cliente.Nombre,
                cliente.Correo,
                cliente.Telefono,
                cliente.Direccion
            };
            var success = await _apiClient.PutAsync($"api/Clientes/{id}", request);
            if (success)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Error al actualizar cliente");
            return View(cliente);
        }

        // GET: /Clientes/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _apiClient.GetAsync<ClienteViewModel>($"api/Clientes/{id}");
            if (cliente == null)
                return NotFound();
            return View(cliente);
        }

        // POST: /Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _apiClient.DeleteAsync($"api/Clientes/{id}");
                TempData["Success"] = "Cliente eliminado correctamente.";
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