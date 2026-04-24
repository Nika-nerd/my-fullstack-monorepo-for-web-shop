namespace SafiShopAPI.DTOs;

public record ProductCreateDto(
    string Name, 
    string Description, 
    decimal BasePrice, 
    decimal? DiscountPrice, 
    string? ImageUrl,
    bool IsPublished,
    List<ProductVariantDto> Variants
);

public record ProductVariantDto(string Size, int StockQuantity);