using Redis_Example_Catalog.Models;

namespace Redis_Example_Catalog.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private static readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Laptop", Price = 1500, Description = "Gaming laptop" },
        new Product { Id = 2, Name = "Mouse", Price = 50, Description = "Wireless mouse" },
        new Product { Id = 3, Name = "Keyboard", Price = 80, Description = "Mechanical keyboard" }
    };

        public Task<List<Product>> GetAllAsync()
        {
            return Task.FromResult(_products);
        }

        public Task<Product?> GetByIdAsync(int id)
        {
            return Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
        }

        public Task UpdateAsync(Product product)
        {
            var existing = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existing == null) return Task.CompletedTask;

            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.Description = product.Description;

            return Task.CompletedTask;
        }
        public Task DeleteAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }

            return Task.CompletedTask;
        }
    }
}
