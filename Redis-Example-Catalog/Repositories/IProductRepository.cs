using Redis_Example_Catalog.Models;

namespace Redis_Example_Catalog.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
    }
}
