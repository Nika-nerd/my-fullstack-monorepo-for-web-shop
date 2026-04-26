import { useProducts } from './hooks/useProduct';

function App() {
  const { products, loading, error } = useProducts();

  if (loading) return <div>Загрузка каталога...</div>;
  if (error) return <div style={{ color: 'red' }}>{error}</div>;

  return (
      <div style={{ padding: '20px' }}>
        <h1>Магазин (Client Interface)</h1>
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(200px, 1fr))', gap: '20px' }}>
          {products.map((product) => (
              <div key={product.id} style={{ border: '1px solid #ccc', padding: '10px', borderRadius: '8px' }}>
                {product.imageUrl && <img src={product.imageUrl} alt={product.name} style={{ width: '100%' }} />}
                <h3>{product.name}</h3>
                <p>{product.description}</p>
                <p>
                  <b>Цена: {product.discountPrice ?? product.basePrice} ₸</b>
                  {product.discountPrice && <span style={{ textDecoration: 'line-through', marginLeft: '10px', color: 'gray' }}>{product.basePrice}</span>}
                </p>

                {/* Выбор размера */}
                <select>
                  {product.variants.map(v => (
                      <option key={v.id} disabled={v.stockQuantity === 0}>
                        {v.size} ({v.stockQuantity} шт.)
                      </option>
                  ))}
                </select>
              </div>
          ))}
        </div>
      </div>
  );
}

export default App;
