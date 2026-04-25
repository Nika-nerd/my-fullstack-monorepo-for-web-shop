using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafiShopAPI.Data;
using SafiShopAPI.Models;
using SafiShopAPI.DTOs;
using SafiShopAPI.Services;

namespace SafiShopAPI.Controllers.Admin;

[ApiController]
[Route("api/admin/products")] 
public class AdminProductsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    private readonly IFileService _fileService;

    public AdminProductsController(ApplicationDbContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
    {
        return await _context.Products
            .Include(p => p.Variants)
            .OrderByDescending(p => p.Id)
            .ToListAsync();
    }

    
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult> CreateProduct(
        [FromForm] string name,
        [FromForm] string description,
        [FromForm] decimal basePrice,
        [FromForm] decimal? discountPrice,
        [FromForm] bool isPublished,
        [FromForm] string variantsJson, 
        IFormFile image)
    {
       
        string imageUrl = await _fileService.SaveImageAsync(image);

        
        var jsonString = variantsJson.Trim();


        if (jsonString.StartsWith("{") && !jsonString.StartsWith("["))
        {
            jsonString = $"[{jsonString}]";
        }

        List<ProductVariantDto> variants;
        try 
        {
            var options = new System.Text.Json.JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true,
               
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString 
            };

            variants = System.Text.Json.JsonSerializer.Deserialize<List<ProductVariantDto>>(jsonString, options) 
                       ?? new List<ProductVariantDto>();
        }
        catch (Exception ex)
        {
            return BadRequest($"Ошибка JSON: {ex.Message}. Итоговая строка: {jsonString}");
        }

       
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            BasePrice = basePrice,
            DiscountPrice = discountPrice,
            IsPublished = isPublished,
            ImageUrl = imageUrl,
            Variants = variants.Select(v => new ProductVariant
            {
                Id = Guid.NewGuid(),
                Size = v.Size,
                StockQuantity = v.StockQuantity
            }).ToList()
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return Ok(product);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}