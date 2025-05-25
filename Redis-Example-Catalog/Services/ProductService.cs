using Redis_Example_Catalog.Models;
using Redis_Example_Catalog.Redis;
using Redis_Example_Catalog.Repositories;
using System.Text.Json;

namespace Redis_Example_Catalog.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly RedisService _redisService;

        public ProductService(IProductRepository productRepository, RedisService redisService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var db = _redisService.Database;
            string cacheKey = $"product:{id}";

            var cachedProduct = await db.StringGetAsync(cacheKey);

            if (cachedProduct.HasValue)
            {
                return JsonSerializer.Deserialize<Product>(cachedProduct!);
            }

            var product = await _productRepository.GetByIdAsync(id);
            if (product != null)
            {
                await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(product),
                    TimeSpan.FromSeconds(60));
            }

            return product;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _productRepository.GetAllAsync(); // Cache'lenmiyor
        }

        public async Task UpdateAsync(Product product)
        {
            await _productRepository.UpdateAsync(product);

            var db = _redisService.Database;
            string cacheKey = $"product:{product.Id}";

            await db.KeyDeleteAsync(cacheKey); // Cache invalidation!
        }

        public async Task DeleteAsync(int id)
        {
            await _productRepository.DeleteAsync(id);

            var db = _redisService.Database;
            string cacheKey = $"product:{id}";
            await db.KeyDeleteAsync(cacheKey); // Redis'ten de sil
        }

        public async Task PrewarmCacheAsync()
        {
            var db = _redisService.Database;

            var topProducts = await _productRepository.GetAllAsync();
            var top3 = topProducts.Take(3);

            foreach (var product in top3)
            {
                string cacheKey = $"product:{product.Id}";
                string json = JsonSerializer.Serialize(product);

                await db.StringSetAsync(cacheKey, json, TimeSpan.FromSeconds(60));
            }
        }
    }
}
