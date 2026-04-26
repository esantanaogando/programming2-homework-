using Impresiones3D.API.Responses;
using Impresiones3D.Application.Requests;
using Impresiones3D.Application.Services;
using Impresiones3D.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Impresiones3D.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialesController : ControllerBase
    {
        private readonly MaterialService _materialService;

        public MaterialesController(MaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var materiales = await _materialService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<Material>>.Ok(materiales));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var material = await _materialService.GetByIdAsync(id);
            if (material == null) return NotFound(ApiResponse<object>.Error("Material no encontrado"));
            return Ok(ApiResponse<Material>.Ok(material));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMaterialRequest request)
        {
            try
            {
                var material = await _materialService.CreateAsync(request);
                return Ok(ApiResponse<Material>.Ok(material, "Material creado"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Error(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateMaterialRequest request)
        {
            try
            {
                await _materialService.UpdateAsync(id, request);
                return Ok(ApiResponse<object>.Ok(null, "Material actualizado"));
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
                await _materialService.DeleteAsync(id);
                return Ok(ApiResponse<object>.Ok(null, "Material eliminado"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Error(ex.Message));
            }
        }
    }
}