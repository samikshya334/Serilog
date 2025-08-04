using DemoApi.Models;
using DemoApi.Repositories;
using Serilog;

namespace DemoApi.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<Product> CreateAsync(Product product)
    {
        _logger.LogInformation("Service: Creating product {@Product}", product);
        var result = await _productRepository.CreateAsync(product);
        if (result != null)
        {
            _logger.LogInformation("Service: Successfully created product with ID {ProductId}", product.Id);
        }
        return result;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        _logger.LogInformation("Service: Retrieving all products");
        var products = await _productRepository.GetAllAsync();
        _logger.LogInformation("Service: Retrieved {ProductCount} products", products.Count);
        return products;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Service: Retrieving product with ID {ProductId}", id);
        var product = await _productRepository.GetByIdAsync(id);
        if (product != null)
        {
            _logger.LogInformation("Service: Successfully retrieved product {@Product}", product);
        }
        return product;
    }

    public async Task<Product?> UpdateAsync(int id, Product product)
    {
        _logger.LogInformation("Service: Updating product with ID {ProductId} to {@Product}", id, product);
        var result = await _productRepository.UpdateAsync(id, product);
        if (result != null)
        {
            _logger.LogInformation("Service: Successfully updated product with ID {ProductId}", id);
        }
        return result;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Service: Deleting product with ID {ProductId}", id);
        var result = await _productRepository.DeleteAsync(id);
        if (result)
        {
            _logger.LogInformation("Service: Successfully deleted product with ID {ProductId}", id);
        }
        return result;
    }
}