using Impresiones3D.Application.Requests;
using Impresiones3D.Domain.Entities;
using Impresiones3D.Domain.Enums;
using Impresiones3D.Domain.Interfaces;

namespace Impresiones3D.Application.Services
{
    public class ImpresoraService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImpresoraService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Impresora>> GetAllAsync()
            => await _unitOfWork.Impresoras.GetAllAsync();

        public async Task<Impresora?> GetByIdAsync(int id)
            => await _unitOfWork.Impresoras.GetByIdAsync(id);

        // NUEVO: asignación automática — elige la mejor impresora disponible
        // Criterio: disponible + misma tecnología + mayor velocidad + menos horas usadas
        public async Task<Impresora> AsignarAutomaticaAsync(TecnologiaImpresora tecnologia)
        {
            var disponibles = await _unitOfWork.Impresoras
                .GetDisponiblesPorTecnologiaAsync(tecnologia);

            var mejor = disponibles
                .OrderByDescending(i => i.VelocidadMaxima)
                .ThenBy(i => i.HorasTotalesUso)
                .FirstOrDefault();

            if (mejor == null)
                throw new Exception($"No hay impresoras {tecnologia} disponibles en este momento");

            mejor.Estado = EstadoImpresora.Ocupada;
            await _unitOfWork.Impresoras.UpdateAsync(mejor);
            await _unitOfWork.CompleteAsync();

            return mejor;
        }

        public async Task<Impresora> CreateAsync(CreateImpresoraRequest request)
        {
            var estadoValido = request.Estado switch
            {
                1 => EstadoImpresora.Disponible,
                2 => EstadoImpresora.Ocupada,
                3 => EstadoImpresora.Mantenimiento,
                _ => EstadoImpresora.Disponible
            };

            var impresora = new Impresora
            {
                Nombre = request.Nombre,
                Modelo = request.Modelo,
                Tecnologia = request.Tecnologia,
                Estado = estadoValido,
                VelocidadMaxima = request.VelocidadMaxima,
                VolumenX = request.VolumenX,
                VolumenY = request.VolumenY,
                VolumenZ = request.VolumenZ
            };

            await _unitOfWork.Impresoras.AddAsync(impresora);
            await _unitOfWork.CompleteAsync();
            return impresora;
        }

        public async Task UpdateAsync(int id, CreateImpresoraRequest request)
        {
            var impresora = await _unitOfWork.Impresoras.GetByIdAsync(id);
            if (impresora == null) throw new Exception("Impresora no encontrada");

            impresora.Nombre = request.Nombre;
            impresora.Modelo = request.Modelo;
            impresora.Tecnologia = (TecnologiaImpresora)request.Tecnologia;
            impresora.Estado = (EstadoImpresora)request.Estado;
            impresora.VelocidadMaxima = request.VelocidadMaxima;
            impresora.VolumenX = request.VolumenX;
            impresora.VolumenY = request.VolumenY;
            impresora.VolumenZ = request.VolumenZ;

            await _unitOfWork.Impresoras.UpdateAsync(impresora);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var impresora = await _unitOfWork.Impresoras.GetByIdAsync(id);
            if (impresora == null)
                throw new Exception("Impresora no encontrada");

            var ordenesAsociadas = await _unitOfWork.Ordenes.FindAsync(o => o.ImpresoraId == id);
            if (ordenesAsociadas.Any())
                throw new Exception("No se puede eliminar una impresora que está siendo usada en órdenes.");

            await _unitOfWork.Impresoras.DeleteAsync(impresora);
            await _unitOfWork.CompleteAsync();
        }

        public async Task CambiarEstadoAsync(int id, EstadoImpresora nuevoEstado)
        {
            var impresora = await _unitOfWork.Impresoras.GetByIdAsync(id);
            if (impresora == null)
                throw new Exception("Impresora no encontrada");

            impresora.Estado = nuevoEstado;
            await _unitOfWork.Impresoras.UpdateAsync(impresora);
            await _unitOfWork.CompleteAsync();
        }
    }
}