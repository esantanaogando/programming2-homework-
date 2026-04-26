using Impresiones3D.Application.DTOs;
using Impresiones3D.Application.Requests;
using Impresiones3D.Domain.Entities;
using Impresiones3D.Domain.Interfaces;

namespace Impresiones3D.Application.Services
{
    public class ClienteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClienteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ClienteDto>> GetAllAsync()
        {
            var clientes = await _unitOfWork.Clientes.GetAllAsync();
            return clientes.Select(ToDto);
        }

        public async Task<ClienteDto?> GetByIdAsync(int id)
        {
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(id);
            return cliente == null ? null : ToDto(cliente);
        }

        // NUEVO: busca por nombre, correo o teléfono (sobrecarga de búsqueda para el profesor)
        public async Task<IEnumerable<ClienteDto>> BuscarAsync(string termino)
        {
            if (string.IsNullOrWhiteSpace(termino))
                return await GetAllAsync();

            var clientes = await _unitOfWork.Clientes.BuscarAsync(termino);
            return clientes.Select(ToDto);
        }

        // NUEVO: historial de órdenes de un cliente
        public async Task<IEnumerable<Orden>> GetOrdenesAsync(int clienteId)
        {
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(clienteId);
            if (cliente == null) throw new Exception("Cliente no encontrado");
            return await _unitOfWork.Ordenes.GetByClienteIdAsync(clienteId);
        }

        public async Task<ClienteDto> CreateAsync(CreateClienteRequest request)
        {
            var existe = await _unitOfWork.Clientes.GetByCorreoAsync(request.Correo);
            if (existe != null)
                throw new Exception("Ya existe un cliente con ese correo electrónico");

            var cliente = new Cliente
            {
                Nombre = request.Nombre,
                Correo = request.Correo,
                Telefono = request.Telefono,
                Direccion = request.Direccion,
                FechaRegistro = DateTime.Now
            };

            await _unitOfWork.Clientes.AddAsync(cliente);
            await _unitOfWork.CompleteAsync();
            return ToDto(cliente);
        }

        public async Task UpdateAsync(int id, CreateClienteRequest request)
        {
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(id);
            if (cliente == null) throw new Exception("Cliente no encontrado");

            cliente.Nombre = request.Nombre;
            cliente.Correo = request.Correo;
            cliente.Telefono = request.Telefono;
            cliente.Direccion = request.Direccion;

            await _unitOfWork.Clientes.UpdateAsync(cliente);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cliente = await _unitOfWork.Clientes.GetByIdWithOrdenesAsync(id);
            if (cliente == null) throw new Exception("Cliente no encontrado");

            if (cliente.Ordenes.Any())
                throw new Exception("No se puede eliminar un material que está siendo usado en órdenes.");

            await _unitOfWork.Clientes.DeleteAsync(cliente);
            await _unitOfWork.CompleteAsync();
        }

        private static ClienteDto ToDto(Cliente c) => new()
        {
            Id = c.Id,
            Nombre = c.Nombre,
            Correo = c.Correo,
            Telefono = c.Telefono,
            Direccion = c.Direccion,
            FechaRegistro = c.FechaRegistro
        };
    }
}