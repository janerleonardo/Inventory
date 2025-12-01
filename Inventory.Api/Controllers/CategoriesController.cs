
using Inventory.Application.DTOs;
using Inventory.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    // GET: api/categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
    {
        try
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener categorías");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    // GET: api/categories/active
    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetActive()
    {
        try
        {
            var categories = await _categoryService.GetActiveAsync();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener categorías activas");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    // GET: api/categories/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetById(Guid id)
    {
        try
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound($"Categoría con ID {id} no encontrada");

            return Ok(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener categoría {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    // POST: api/categories
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _categoryService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear categoría");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    // PUT: api/categories/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryDto>> Update(Guid id, [FromBody] UpdateCategoryDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _categoryService.UpdateAsync(id, dto);
            if (category == null)
                return NotFound($"Categoría con ID {id} no encontrada");

            return Ok(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar categoría {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    // DELETE: api/categories/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            var result = await _categoryService.DeleteAsync(id);
            if (!result)
                return NotFound($"Categoría con ID {id} no encontrada");

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar categoría {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }
}