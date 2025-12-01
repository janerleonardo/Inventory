
using Inventory.Application.DTOs;
using Inventory.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;
    private readonly ILogger<SuppliersController> _logger;

    public SuppliersController(ISupplierService supplierService, ILogger<SuppliersController> logger)
    {
        _supplierService = supplierService;
        _logger = logger;
    }

    // GET: api/suppliers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SupplierDto>>> GetAll()
    {
        try
        {
            var suppliers = await _supplierService.GetAllAsync();
            return Ok(suppliers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener proveedores");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    // GET: api/suppliers/active
    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<SupplierDto>>> GetActive()
    {
        try
        {
            var suppliers = await _supplierService.GetActiveAsync();
            return Ok(suppliers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener proveedores activos");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    // GET: api/suppliers/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<SupplierDto>> GetById(Guid id)
    {
        try
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            if (supplier == null)
                return NotFound($"Proveedor con ID {id} no encontrado");

            return Ok(supplier);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener proveedor {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    // POST: api/suppliers
    [HttpPost]
    public async Task<ActionResult<SupplierDto>> Create([FromBody] CreateSupplierDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var supplier = await _supplierService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = supplier.Id }, supplier);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear proveedor");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    // PUT: api/suppliers/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<SupplierDto>> Update(Guid id, [FromBody] UpdateSupplierDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var supplier = await _supplierService.UpdateAsync(id, dto);
            if (supplier == null)
                return NotFound($"Proveedor con ID {id} no encontrado");

            return Ok(supplier);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar proveedor {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    // DELETE: api/suppliers/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            var result = await _supplierService.DeleteAsync(id);
            if (!result)
                return NotFound($"Proveedor con ID {id} no encontrado");

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar proveedor {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }
}