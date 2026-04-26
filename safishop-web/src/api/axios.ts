import axios from 'axios';
import type { Product, CreateOrderDto } from '../types/api';

const instance = axios.create({
    baseURL: 'http://localhost:5075/api', // Твой порт
    headers: { 'Content-Type': 'application/json' }
});

// Мы экспортируем этот объект именованно (через {})
export const api = {
    products: {
        getCatalog: () => instance.get<Product[]>('/products'),
        checkout: (data: CreateOrderDto) => instance.post('/products/checkout', data)
    }
};

// Это можно оставить, но в useProducts мы его НЕ используем
export default instance;