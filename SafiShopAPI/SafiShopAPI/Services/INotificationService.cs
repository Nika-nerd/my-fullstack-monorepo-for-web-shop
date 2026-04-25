using Microsoft.Extensions.Options;
using SafiShopAPI.Models;
using System.Net.Http.Json; 

namespace SafiShopAPI.Services; 

public interface INotificationService
{
    Task SendOrderNotificationAsync(Order order);
}

public class TelegramNotificationService : INotificationService
{
    private readonly HttpClient _httpClient;
    private readonly TelegramSettings _settings;

    public TelegramNotificationService(HttpClient httpClient, IOptions<TelegramSettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value;
    }

    public async Task SendOrderNotificationAsync(Order order)
    {
        var itemsMsg = string.Join("\n", order.Items.Select(i => $"• {i.ProductName} ({i.Size}) x{i.Quantity}"));
        
        var text = $"🛍 *Новый заказ в SafiShop!*\n\n" +
                   $"👤 Имя: `{order.CustomerName}`\n" +
                   $"📞 Тел: `{order.CustomerPhone}`\n" +
                   $"💰 Сумма: *{order.TotalAmount:N0} KZT*\n\n" +
                   $"📦 Товары:\n{itemsMsg}";

        await _httpClient.PostAsJsonAsync($"https://api.telegram.org/bot{_settings.BotToken}/sendMessage", new {
            chat_id = _settings.AdminChatId,
            text = text,
            parse_mode = "Markdown"
        });
    }
}