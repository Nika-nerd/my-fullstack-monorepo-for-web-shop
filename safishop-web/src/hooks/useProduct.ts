import { useState, useEffect } from 'react';
import { api } from '../api/axios'
import type { Product } from '../types/api';

export const useProducts = () => {
    const [products, setProducts] = useState<Product[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const fetchProducts = async () => {
        try {
            setLoading(true);
            const response = await api.products.getCatalog();
            setProducts(response.data);
            setError(null);
        } catch (err) {
            setError('Не удалось загрузить товары. Проверь бэкенд!');
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchProducts();
    }, []);

    return { products, loading, error, refresh: fetchProducts };
};