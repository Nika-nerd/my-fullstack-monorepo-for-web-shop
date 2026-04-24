using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafiShopAPI.Data;
using SafiShopAPI.Models;
using SafiShopAPI.DTOs;

namespace SafiShopAPI.Controllers.Admin;

[ApiController]
[Route("api/admin/products")] 
public class AdminProductsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AdminProductsController(ApplicationDbContext context)
    {
        _context = context;
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
    public async Task<ActionResult<Product>> CreateProduct(ProductCreateDto dto)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            BasePrice = dto.BasePrice,
            DiscountPrice = dto.DiscountPrice,
            ImageUrl = dto.ImageUrl,
            IsPublished = dto.IsPublished,
            Variants = dto.Variants.Select(v => new ProductVariant
            {
                Id = Guid.NewGuid(),
                Size = v.Size,
                StockQuantity = v.StockQuantity
            }).ToList()
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAllProducts), new { id = product.Id }, product);
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