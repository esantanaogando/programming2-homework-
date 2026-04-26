using Impresiones3D.API.Responses;
using Impresiones3D.Application.DTOs;
using Impresiones3D.Application.Requests;
using Impresiones3D.Application.Services;
using Impresiones3D.Domain.Entities;
using Impresiones3D.Domain.Enums;
using Impresiones3D.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Impresiones3D.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdenesController : ControllerBase
    {
        private readonly OrdenService _ordenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ConfiguracionService _configuracionService;

        public OrdenesController(
            OrdenService ordenService,
            IUnitOfWork unitOfWork,
            ConfiguracionService configuracionService)
        {
            _ordenService = ordenService;
            _unitOfWork = unitOfWork;
            _configuracionService = configuracionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ordenes = await _ordenService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<OrdenDto>>.Ok(ordenes));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var orden = await _ordenService.GetByIdAsync(id);
            if (orden == null) return NotFound(ApiResponse<object>.Error("Orden no encontrada"));
            return Ok(ApiResponse<OrdenDto>.Ok(orden));
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar(
            [FromQuery] int? clienteId,
            [FromQuery] DateTime? desde,
            [FromQuery] DateTime? hasta,
            [FromQuery] EstadoOrden? estado,
            [FromQuery] int? materialId)
        {
            var ordenes = await _ordenService.BuscarAsync(clienteId, desde, hasta, estado, materialId);
            return Ok(ApiResponse<IEnumerable<OrdenDto>>.Ok(ordenes));
        }

        [HttpGet("{id}/comprobante")]
        public async Task<IActionResult> GetComprobante(int id)
        {
            var orden = await _unitOfWork.Ordenes.GetByIdAsync(id);
            if (orden == null || orden.Estado != EstadoOrden.Entregada)
                return NotFound("Orden no encontrada o no entregada");

            var cliente = await _unitOfWork.Clientes.GetByIdAsync(orden.ClienteId);
            var material = await _unitOfWork.Materiales.GetByIdAsync(orden.MaterialId);

            var contenido = $@"
COMPROBANTE DE ENTREGA - IMPRESIÓN 3D
=====================================
Número de orden: {orden.Id}
Cliente:         {cliente?.Nombre}
Material:        {material?.Nombre}
Cantidad:        {orden.CantidadPiezas} piezas
Costo:           ${orden.CostoEstimado:N2}
Fecha solicitud: {orden.FechaSolicitud:dd/MM/yyyy}
Fecha entrega:   {orden.FechaEntrega:dd/MM/yyyy}
Estado:          Entregada
-------------------------------------
Gracias por confiar en 3DPrintFlow
";
            var bytes = System.Text.Encoding.UTF8.GetBytes(contenido);
            return File(bytes, "text/plain", $"Comprobante_Orden_{orden.Id}.txt");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrdenRequest request)
        {
            try
            {
                var orden = await _ordenService.CreateAsync(request);
                return Ok(ApiResponse<OrdenDto>.Ok(orden, "Orden creada exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Error(ex.Message));
            }
        }

        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(
            int id,
            [FromQuery] EstadoOrden nuevoEstado,
            [FromQuery] string? motivo = null,
            [FromQuery] int? impresoraId = null)
        {
            try
            {
                if (nuevoEstado == EstadoOrden.EnProduccion && impresoraId == null)
                    return BadRequest(ApiResponse<object>.Error("Debe asignar una impresora para iniciar producción"));

                await _ordenService.CambiarEstadoAsync(id, nuevoEstado, motivo, impresoraId);
                return Ok(ApiResponse<object>.Ok(null, "Estado actualizado"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Error(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOrdenRequest request)
        {
            try
            {
                var orden = await _unitOfWork.Ordenes.GetByIdAsync(id);
                if (orden == null)
                    return NotFound(ApiResponse<object>.Error("Orden no encontrada"));

                if (orden.Estado == EstadoOrden.Entregada || orden.Estado == EstadoOrden.Cancelada)
                    return BadRequest(ApiResponse<object>.Error("No se puede editar una orden entregada o cancelada"));

                orden.ClienteId = request.ClienteId;
                orden.MaterialId = request.MaterialId;
                orden.ImpresoraId = request.ImpresoraId;
                orden.DescripcionModelo = request.DescripcionModelo;
                orden.CantidadPiezas = request.CantidadPiezas;
                orden.AlturaCapa = request.AlturaCapa;
                orden.Relleno = request.Relleno;

                var costoPorHora = request.CostoPorHoraPersonalizado.HasValue
                    ? request.CostoPorHoraPersonalizado.Value
                    : await _configuracionService.ObtenerCostoPorHora();
                orden.CalcularEstimados(costoPorHora);

                await _unitOfWork.Ordenes.UpdateAsync(orden);
                await _unitOfWork.CompleteAsync();

                return Ok(ApiResponse<Orden>.Ok(orden, "Orden actualizada correctamente"));
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
                await _ordenService.DeleteAsync(id);
                return Ok(ApiResponse<object>.Ok(null, "Orden eliminada correctamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Error(ex.Message));
            }
        }
    }
}