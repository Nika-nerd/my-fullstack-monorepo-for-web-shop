

export const OrderStatus = {
    Pending: 0,
    Paid: 1,
    Cancelled: 2
} as const;


export type OrderStatusType = (typeof OrderStatus)[keyof typeof OrderStatus];

export interface ProductVariant {
    id: string;
    size: string;
    stockQuantity: number;
    productId: string;
}

export interface Product {
    id: string;
    name: string;
    description: string;
    imageUrl?: string;
    basePrice: number;
    discountPrice?: number;
    isPublished: boolean;
    variants: ProductVariant[];
}

export interface OrderItemRequest {
    productVariantId: string;
    quantity: number;
}

export interface CreateOrderDto {
    customerName: string;
    customerPhone: string;
    items: OrderItemRequest[];
}