using Impresiones3D.Domain.Entities;
using Impresiones3D.Domain.Interfaces;

namespace Impresiones3D.Application.Services
{
    public class ConfiguracionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConfiguracionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string?> ObtenerValor(string clave)
        {
            var config = await _unitOfWork.Configuraciones.GetByClaveAsync(clave);
            return config?.Valor;
        }

        public async Task<decimal> ObtenerCostoPorHora()
        {
            var valor = await ObtenerValor("CostoPorHora");
            return decimal.TryParse(valor, out var resultado) ? resultado : 5m;
        }

        public async Task ActualizarConfiguracion(string clave, string nuevoValor)
        {
            var config = await _unitOfWork.Configuraciones.GetByClaveAsync(clave);
            if (config != null)
            {
                config.Valor = nuevoValor;
                await _unitOfWork.Configuraciones.UpdateAsync(config);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}