using Impresiones3D.Application.DTOs;
using Impresiones3D.Application.Requests;
using Impresiones3D.Domain.Entities;
using Impresiones3D.Domain.Enums;
using Impresiones3D.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Impresiones3D.Application.Services
{
    public class OrdenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ConfiguracionService _configuracionService;

        public OrdenService(IUnitOfWork unitOfWork, ConfiguracionService configuracionService)
        {
            _unitOfWork = unitOfWork;
            _configuracionService = configuracionService;
        }

        private static OrdenDto ToDto(Orden o) => new()
        {
            Id = o.Id,
            ClienteId = o.ClienteId,
            ClienteNombre = o.Cliente?.Nombre ?? "N/A",
            MaterialId = o.MaterialId,
            MaterialNombre = o.Material?.Nombre ?? "N/A",
            ImpresoraId = o.ImpresoraId,
            ImpresoraNombre = o.Impresora?.Nombre ?? "No asignada",
            DescripcionModelo = o.DescripcionModelo,
            CantidadPiezas = o.CantidadPiezas,
            AlturaCapa = o.AlturaCapa,
            Relleno = o.Relleno,
            TiempoEstimadoHoras = o.TiempoEstimadoHoras,
            CostoEstimado = o.CostoEstimado,
            Estado = o.Estado.ToString(),
            FechaSolicitud = o.FechaSolicitud,
            FechaEntrega = o.FechaEntrega,
            MotivoCancelacion = o.MotivoCancelacion
        };

        public async Task<IEnumerable<OrdenDto>> GetAllAsync()
        {
            var ordenes = await _unitOfWork.Ordenes.GetAllWithIncludesAsync();
            return ordenes.Select(ToDto);
        }

        public async Task<OrdenDto?> GetByIdAsync(int id)
        {
            var orden = await _unitOfWork.Ordenes.GetByIdWithIncludesAsync(id);
            return orden == null ? null : ToDto(orden);
        }

        public async Task<IEnumerable<OrdenDto>> BuscarAsync(
            int? clienteId, DateTime? desde, DateTime? hasta,
            EstadoOrden? estado, int? materialId)
        {
            var ordenes = await _unitOfWork.Ordenes.BuscarAsync(clienteId, desde, hasta, estado, materialId);
            return ordenes.Select(ToDto);
        }

        public async Task<OrdenDto> CreateAsync(CreateOrdenRequest request)
        {
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(request.ClienteId);
            if (cliente == null) throw new Exception("Cliente no existe");

            var material = await _unitOfWork.Materiales.GetByIdAsync(request.MaterialId);
            if (material == null) throw new Exception("Material no existe");
            if (material.Stock < request.CantidadPiezas) throw new Exception("Stock insuficiente");

            var orden = new Orden
            {
                ClienteId = request.ClienteId,
                MaterialId = request.MaterialId,
                DescripcionModelo = request.DescripcionModelo,
                CantidadPiezas = request.CantidadPiezas,
                AlturaCapa = request.AlturaCapa,
                Relleno = request.Relleno,
                Estado = EstadoOrden.Pendiente
            };

            var costoPorHora = await _configuracionService.ObtenerCostoPorHora();
            orden.CalcularEstimados(costoPorHora);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.Ordenes.AddAsync(orden);
                material.Stock -= orden.CantidadPiezas;
                await _unitOfWork.Materiales.UpdateAsync(material);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            return ToDto(orden);
        }

        public async Task DeleteAsync(int id)
        {
            var orden = await _unitOfWork.Ordenes.GetByIdAsync(id);
            if (orden == null) throw new Exception("Orden no encontrada");

            // Solo permitir eliminar si está Cancelada o Entregada
            if (orden.Estado != EstadoOrden.Cancelada && orden.Estado != EstadoOrden.Entregada)
                throw new Exception("Solo se pueden eliminar órdenes en estado Cancelada o Entregada");

            await _unitOfWork.Ordenes.DeleteAsync(orden);
            await _unitOfWork.CompleteAsync();
        }

        public async Task CambiarEstadoAsync(
            int id, EstadoOrden nuevoEstado,
            string? motivoCancelacion = null, int? impresoraId = null)
        {
            var orden = await _unitOfWork.Ordenes.GetByIdAsync(id);
            if (orden == null) throw new Exception("Orden no encontrada");

            if (orden.Estado == EstadoOrden.Entregada || orden.Estado == EstadoOrden.Cancelada)
                throw new Exception("No se puede cambiar el estado de una orden entregada o cancelada");

            if (nuevoEstado == EstadoOrden.Cancelada && string.IsNullOrWhiteSpace(motivoCancelacion))
                throw new Exception("Debe proporcionar un motivo para cancelar la orden");

            if (nuevoEstado == EstadoOrden.EnProduccion && !impresoraId.HasValue)
                throw new Exception("Debe asignar una impresora para iniciar producción");

            if (impresoraId.HasValue)
                orden.ImpresoraId = impresoraId.Value;

            switch (nuevoEstado)
            {
                case EstadoOrden.EnProduccion:
                    orden.FechaInicioProduccion = DateTime.Now;
                    break;
                case EstadoOrden.Completada:
                    orden.FechaFinProduccion = DateTime.Now;
                    if (orden.ImpresoraId.HasValue)
                    {
                        var impresora = await _unitOfWork.Impresoras.GetByIdAsync(orden.ImpresoraId.Value);
                        if (impresora != null)
                        {
                            impresora.HorasTotalesUso += orden.TiempoEstimadoHoras;
                            impresora.Estado = EstadoImpresora.Disponible;
                            await _unitOfWork.Impresoras.UpdateAsync(impresora);
                        }
                    }
                    break;
                case EstadoOrden.Entregada:
                    orden.FechaEntrega = DateTime.Now;
                    break;
                case EstadoOrden.Cancelada:
                    orden.MotivoCancelacion = motivoCancelacion;
                    if (orden.ImpresoraId.HasValue)
                    {
                        var impresora = await _unitOfWork.Impresoras.GetByIdAsync(orden.ImpresoraId.Value);
                        if (impresora != null && impresora.Estado == EstadoImpresora.Ocupada)
                            impresora.Estado = EstadoImpresora.Disponible;
                        await _unitOfWork.Impresoras.UpdateAsync(impresora);
                    }
                    break;
            }

            orden.Estado = nuevoEstado;
            await _unitOfWork.Ordenes.UpdateAsync(orden);
            await _unitOfWork.CompleteAsync();
        }
    }
}



