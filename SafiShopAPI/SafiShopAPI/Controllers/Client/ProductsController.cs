using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafiShopAPI.Data;
using SafiShopAPI.Models;

namespace SafiShopAPI.Controllers.Client;

[ApiController]
[Route("api/products")] 
public class ProductsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetCatalog()
    {
        return await _context.Products
            .Include(p => p.Variants)
            .Where(p => p.IsPublished) 
            .AsNoTracking() 
            .ToListAsync();
    }

    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Product>> GetProduct(Guid id)
    {
        var product = await _context.Products
            .Include(p => p.Variants)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id && p.IsPublished);

        if (product == null) return NotFound();

        return product;
    }
}