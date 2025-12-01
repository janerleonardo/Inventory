using Inventory.Application.DTOs;
using Inventory.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    // GET: api/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
    {
        try
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener productos");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    // GET: api/products/active
    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetActive()
    {
        try
        {
            var products = await _productService.GetActiveProductsAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener productos activos");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    // GET: api/products/low-stock
    [HttpGet("low-stock")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetLowStock()
    {
        try
        {
            var products = await _productService.GetLowStockProductsAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener productos con stock bajo");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    // GET: api/products/category/{categoryId}
    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetByCategory(Guid categoryId)
    {
        try
        {
            var products = await _productService.GetByCategoryAsync(categoryId);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener productos por categoría {CategoryId}", categoryId);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    // GET: api/products/supplier/{supplierId}
    [HttpGet("supplier/{supplierId}")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetBySupplier(Guid supplierId)
    {
        try
        {
            var products = await _productService.GetBySupplierAsync(supplierId);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener productos por proveedor {SupplierId}", supplierId);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    // GET: api/products/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(Guid id)
    {
        try
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound($"Producto con ID {id} no encontrado");

            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener producto {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear producto");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    // PUT: api/products/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDto>> Update(Guid id, [FromBody] UpdateProductDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productService.UpdateAsync(id, dto);
            if (product == null)
                return NotFound($"Producto con ID {id} no encontrado");

            return Ok(product);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar producto {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            var result = await _productService.DeleteAsync(id);
            if (!result)
                return NotFound($"Producto con ID {id} no encontrado");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar producto {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    // POST: api/products/{id}/decrement-stock
    [HttpPost("{id}/decrement-stock")]
    public async Task<ActionResult<ProductDto>> DecrementStock(Guid id, [FromBody] AdjustStockDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productService.DecrementStockAsync(id, dto.Amount);
            if (product == null)
                return NotFound($"Producto con ID {id} no encontrado");

            return Ok(product);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al decrementar stock del producto {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    // POST: api/products/{id}/increment-stock
    [HttpPost("{id}/increment-stock")]
    public async Task<ActionResult<ProductDto>> IncrementStock(Guid id, [FromBody] AdjustStockDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productService.IncrementStockAsync(id, dto.Amount);
            if (product == null)
                return NotFound($"Producto con ID {id} no encontrado");

            return Ok(product);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al incrementar stock del producto {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }
}