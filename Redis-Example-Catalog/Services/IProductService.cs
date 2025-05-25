using Redis_Example_Catalog.Models;

namespace Redis_Example_Catalog.Services
{
    public interface IProductService
    {
        Task<Product?> GetByIdAsync(int id);
        Task<List<Product>> GetAllAsync();
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task PrewarmCacheAsync();
    }
}
