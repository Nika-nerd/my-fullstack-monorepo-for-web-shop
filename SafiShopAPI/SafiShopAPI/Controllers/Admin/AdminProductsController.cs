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
    [Consumes("multipart/form-data")] // Указываем Swagger-у, что это форма с файлом
    public async Task<ActionResult> CreateProduct([FromForm] ProductCreateDto dto, IFormFile image)
    {
        
        string imageUrl = await _fileService.SaveImageAsync(image);

        // 2. Создаем модель товара
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            BasePrice = dto.BasePrice,
            DiscountPrice = dto.DiscountPrice,
            IsPublished = dto.IsPublished,
            ImageUrl = imageUrl, // Ссылка на файл
            Variants = dto.Variants.Select(v => new ProductVariant
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