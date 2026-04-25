namespace SafiShopAPI.Models;
    
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public decimal BasePrice { get; set; }
        public decimal? DiscountPrice { get; set; } 
        public bool IsPublished { get; set; } = false; 
        
        public List<ProductVariant> Variants { get; set; } = new();
    }
    
    public class ProductVariant
    {
        public Guid Id { get; set; }
        public string Size { get; set; } = string.Empty; // S, M, L, XL
        public int StockQuantity { get; set; }
        public Guid ProductId { get; set; }

        public Product Product { get; set; } = null!;
    }


    public class TelegramSettings
    {
        public string BotToken { get; set; } = string.Empty;
        public string AdminChatId { get; set; } = string.Empty;
    }

