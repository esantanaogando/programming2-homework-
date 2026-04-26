using Impresiones3D.API.Responses;
using Impresiones3D.Application.DTOs;
using Impresiones3D.Application.Requests;
using Impresiones3D.Application.Services;
using Impresiones3D.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Impresiones3D.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClientesController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _clienteService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<ClienteDto>>.Ok(clientes));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);
            if (cliente == null) return NotFound(ApiResponse<object>.Error("Cliente no encontrado"));
            return Ok(ApiResponse<ClienteDto>.Ok(cliente));
        }

        // NUEVO: búsqueda por nombre, correo o teléfono
        // GET api/Clientes/buscar?termino=juan
        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string termino)
        {
            var clientes = await _clienteService.BuscarAsync(termino);
            return Ok(ApiResponse<IEnumerable<ClienteDto>>.Ok(clientes));
        }

        // NUEVO: historial completo de órdenes de un cliente
        // GET api/Clientes/5/ordenes
        [HttpGet("{id}/ordenes")]
        public async Task<IActionResult> GetOrdenes(int id)
        {
            try
            {
                var ordenes = await _clienteService.GetOrdenesAsync(id);
                return Ok(ApiResponse<IEnumerable<Orden>>.Ok(ordenes));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Error(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClienteRequest request)
        {
            try
            {
                var cliente = await _clienteService.CreateAsync(request);
                return Ok(ApiResponse<ClienteDto>.Ok(cliente, "Cliente creado exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Error(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateClienteRequest request)
        {
            try
            {
                await _clienteService.UpdateAsync(id, request);
                return Ok(ApiResponse<object>.Ok(null, "Cliente actualizado"));
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
                await _clienteService.DeleteAsync(id);
                return Ok(ApiResponse<object>.Ok(null, "Cliente eliminado"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Error(ex.Message));
            }
        }
    }
}