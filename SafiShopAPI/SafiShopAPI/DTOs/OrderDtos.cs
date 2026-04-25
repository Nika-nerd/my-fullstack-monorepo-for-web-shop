using System;
using System.Collections.Generic;

namespace SafiShopAPI.DTOs;

public record CreateOrderDto(
    string CustomerName,
    string CustomerPhone,
    List<OrderItemRequestDto> Items
);

public record OrderItemRequestDto(
    Guid ProductVariantId, 
    int Quantity
);