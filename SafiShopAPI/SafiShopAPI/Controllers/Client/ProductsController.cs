using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafiShopAPI.Data;
using SafiShopAPI.Models;
using SafiShopAPI.DTOs;
using SafiShopAPI.Services;

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

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout(
        [FromBody] CreateOrderDto dto,
        [FromServices] INotificationService notificationService)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerName = dto.CustomerName,
            CustomerPhone = dto.CustomerPhone,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        decimal total = 0;

        foreach (var item in dto.Items)
        {

            var variant = await _context.ProductVariants
                .Include(v => v.Product)
                .FirstOrDefaultAsync(v => v.Id == item.ProductVariantId);

            if (variant == null) return BadRequest("Товар не найден");
            if (variant.StockQuantity < item.Quantity) return BadRequest($"Недостаточно {variant.Product.Name}");

            var price = variant.Product.DiscountPrice ?? variant.Product.BasePrice;

            order.Items.Add(new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductVariantId = variant.Id,
                ProductName = variant.Product.Name,
                Size = variant.Size,
                Quantity = item.Quantity,
                PriceAtPurchase = price
            });

            total += price * item.Quantity;
        }

        order.TotalAmount = total;
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();


        await notificationService.SendOrderNotificationAsync(order);

        return Ok(new { message = "Заказ создан, уведомление отправлено", orderId = order.Id });
    }
}