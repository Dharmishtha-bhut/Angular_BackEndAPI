using Backend.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<IReadOnlyList<Product>> GetProductsAsync(string? brand,string? type,string? sort);

        Task<Product?> GetProductByIdAsync(int id);

        void AddProductAsync(Product product);

        void UpdateProductAsync(Product product);

        void DeleteProductAsync(Product product);

        bool ProductExistsAsync(int id);

        Task<bool> SaveChangesAsync();

        Task<IReadOnlyList<string>> GetBrandsAsync();

        Task<IReadOnlyList<string>> GetTypesAsync();
    }
}
