using Microsoft.AspNetCore.Mvc;
using ProductApp.Models;

namespace ProductApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ILogger<ProductsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetAllProducts()
    {
        var products = new List<Product>()
        {
            new Product() {Id = 1 , ProductName = "Laptop"},
            new Product() {Id = 2 , ProductName = "Mouse"},
            new Product() {Id = 3 , ProductName = "RGB Keyboard"},
        };

        _logger.LogInformation("GetAllProducts action has been called.");

        return Ok(products);
    }

    [HttpPost]
    public IActionResult AddProduct([FromBody] Product product)
    {
        _logger.LogWarning("Product has been created!");
        return StatusCode(201);
    }
}
