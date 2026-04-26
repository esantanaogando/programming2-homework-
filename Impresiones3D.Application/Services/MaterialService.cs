using Impresiones3D.Application.Requests;
using Impresiones3D.Domain.Entities;
using Impresiones3D.Domain.Interfaces;

namespace Impresiones3D.Application.Services
{
    public class MaterialService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MaterialService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Material>> GetAllAsync() => await _unitOfWork.Materiales.GetAllAsync();
        public async Task<Material?> GetByIdAsync(int id) => await _unitOfWork.Materiales.GetByIdAsync(id);

        public async Task<Material> CreateAsync(CreateMaterialRequest request)
        {
            var material = new Material
            {
                Nombre = request.Nombre,
                Color = request.Color,
                PrecioPorGramo = request.PrecioPorGramo,
                Stock = request.Stock,
                UnidadMedida = request.UnidadMedida ?? "gramos"
            };
            await _unitOfWork.Materiales.AddAsync(material);
            await _unitOfWork.CompleteAsync();
            return material;
        }

        public async Task UpdateAsync(int id, CreateMaterialRequest request)
        {
            var material = await _unitOfWork.Materiales.GetByIdAsync(id);
            if (material == null) throw new Exception("Material no encontrado");
            material.Nombre = request.Nombre;
            material.Color = request.Color;
            material.PrecioPorGramo = request.PrecioPorGramo;
            material.Stock = request.Stock;
            material.UnidadMedida = request.UnidadMedida ?? "gramos";
            await _unitOfWork.Materiales.UpdateAsync(material);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var material = await _unitOfWork.Materiales.GetByIdAsync(id);
            if (material == null) throw new Exception("Material no encontrado");

            // Verificar si está asociado a alguna orden
            var existeEnOrden = await _unitOfWork.Ordenes.ExisteOrdenConMaterialAsync(id);
            if (existeEnOrden)
                throw new Exception("No se puede eliminar un material que está siendo usado en órdenes.");

            await _unitOfWork.Materiales.DeleteAsync(material);
            await _unitOfWork.CompleteAsync();
        }
    }
}