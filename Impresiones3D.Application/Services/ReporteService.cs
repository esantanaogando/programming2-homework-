using Impresiones3D.Application.DTOs;
using Impresiones3D.Domain.Enums;
using Impresiones3D.Domain.Interfaces;
using System.Text;

namespace Impresiones3D.Application.Services
{
    public class ReporteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReporteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // --- REPORTE 1: Materiales más usados ---
        public async Task<IEnumerable<MaterialMasUsadoDto>> GetMaterialesMasUsadosAsync()
        {
            var ordenes = await _unitOfWork.Ordenes.FindAsync(
                o => o.Estado == EstadoOrden.Entregada || o.Estado == EstadoOrden.Completada,
                o => o.Material
            );

            return ordenes
                .GroupBy(o => o.MaterialId)
                .Select(g => new MaterialMasUsadoDto
                {
                    MaterialNombre = g.First().Material?.Nombre ?? "Desconocido",
                    TotalUsos = g.Count(),
                    CantidadTotalStockUsado = g.Sum(o => o.CantidadPiezas)
                })
                .OrderByDescending(x => x.TotalUsos)
                .ToList();
        }

        // --- REPORTE 2: Ingresos proyectados vs reales ---
        public async Task<IEnumerable<IngresoReporteDto>> GetIngresosAsync(
            DateTime? desde = null, DateTime? hasta = null)
        {
            var ordenes = await _unitOfWork.Ordenes.FindAsync(
                o => (!desde.HasValue || o.FechaSolicitud >= desde.Value)
                  && (!hasta.HasValue || o.FechaSolicitud <= hasta.Value.AddDays(1)),
                o => o.Cliente,
                o => o.Material
            );

            return ordenes
                .OrderByDescending(o => o.FechaSolicitud)
                .Select(o => new IngresoReporteDto
                {
                    OrdenId = o.Id,
                    ClienteNombre = o.Cliente?.Nombre ?? "-",
                    MaterialNombre = o.Material?.Nombre ?? "-",
                    CostoEstimado = o.CostoEstimado,
                    // Solo cuenta como ingreso real si la orden fue entregada
                    CostoReal = o.Estado == EstadoOrden.Entregada ? o.CostoEstimado : 0,
                    Estado = o.Estado.ToString(),
                    FechaSolicitud = o.FechaSolicitud,
                    FechaEntrega = o.FechaEntrega
                })
                .ToList();
        }

        // --- REPORTE 3: Tiempo total de uso por impresora ---
        public async Task<IEnumerable<TiempoUsoImpresoraDto>> GetTiempoUsoImpresorasAsync()
        {
            var impresoras = await _unitOfWork.Impresoras.GetAllAsync();
            var ordenes = await _unitOfWork.Ordenes.GetAllAsync();

            return impresoras
                .Select(i => new TiempoUsoImpresoraDto
                {
                    ImpresoraId = i.Id,
                    ImpresoraNombre = i.Nombre,
                    Modelo = i.Modelo,
                    Tecnologia = i.Tecnologia.ToString(),
                    Estado = i.Estado.ToString(),
                    HorasTotalesUso = i.HorasTotalesUso,
                    TotalOrdenes = ordenes.Count(o => o.ImpresoraId == i.Id)
                })
                .OrderByDescending(x => x.HorasTotalesUso)
                .ToList();
        }

        // --- CSV: Materiales más usados ---
        public async Task<byte[]> ExportarMaterialesCsvAsync()
        {
            var data = await GetMaterialesMasUsadosAsync();
            var sb = new StringBuilder();
            sb.AppendLine("Material,Total Usos,Cantidad Total Usada");
            foreach (var r in data)
                sb.AppendLine($"{r.MaterialNombre},{r.TotalUsos},{r.CantidadTotalStockUsado}");
            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        // --- CSV: Ingresos ---
        public async Task<byte[]> ExportarIngresosCsvAsync(DateTime? desde, DateTime? hasta)
        {
            var data = await GetIngresosAsync(desde, hasta);
            var sb = new StringBuilder();
            sb.AppendLine("Orden,Cliente,Material,Costo Estimado,Costo Real,Estado,Fecha Solicitud,Fecha Entrega");
            foreach (var r in data)
                sb.AppendLine(
                    $"{r.OrdenId},{r.ClienteNombre},{r.MaterialNombre}," +
                    $"{r.CostoEstimado},{r.CostoReal},{r.Estado}," +
                    $"{r.FechaSolicitud:dd/MM/yyyy},{r.FechaEntrega:dd/MM/yyyy}");
            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        // --- CSV: Tiempo uso impresoras ---
        public async Task<byte[]> ExportarTiempoUsoImpresorasCsvAsync()
        {
            var data = await GetTiempoUsoImpresorasAsync();
            var sb = new StringBuilder();
            sb.AppendLine("Impresora,Modelo,Tecnologia,Estado,Horas Totales,Total Ordenes");
            foreach (var r in data)
                sb.AppendLine(
                    $"{r.ImpresoraNombre},{r.Modelo},{r.Tecnologia}," +
                    $"{r.Estado},{r.HorasTotalesUso},{r.TotalOrdenes}");
            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}