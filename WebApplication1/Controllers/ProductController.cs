using DemoApi.Models;
using DemoApi.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;


namespace DemoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductService productService, ILogger<ProductController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        try
        {
            _logger.LogInformation("Received request to create product {@Product}", product);
            var createdProduct = await _productService.CreateAsync(product);
            if (createdProduct == null)
            {
                _logger.LogWarning("Failed to create product with ID {ProductId}: ID already exists", product.Id);
                return BadRequest("Product ID already exists");
            }
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product with ID {ProductId}", product.Id);
            return StatusCode(500, "Internal server error");
        }
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var products = await _productService.GetAllAsync();
            _logger.LogInformation("Retrieved all products, count: {ProductCount}", products.Count);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all products");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found", id);
                return NotFound();
            }
            _logger.LogInformation("Retrieved product {@Product}", product);
            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product with ID {ProductId}", id);
            return StatusCode(500, "Internal server error");
        }
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Product product)
    {
        try
        {
            _logger.LogInformation("Received request to update product with ID {ProductId} to {@Product}", id, product);
            var updatedProduct = await _productService.UpdateAsync(id, product);
            if (updatedProduct == null)
            {
                _logger.LogWarning("Failed to update product with ID {ProductId}: Not found", id);
                return NotFound();
            }
            return Ok(updatedProduct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product with ID {ProductId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var success = await _productService.DeleteAsync(id);
            if (!success)
            {
                _logger.LogWarning("Failed to delete product with ID {ProductId}: Not found", id);
                return NotFound();
            }
            _logger.LogInformation("Deleted product with ID {ProductId}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product with ID {ProductId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}