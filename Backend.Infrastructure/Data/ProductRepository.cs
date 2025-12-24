using Backend.Core.Entities;
using Backend.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Backend.Infrastructure.Data
{
    public class ProductRepository(StoreContext context) : IProductRepository
    {
        public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
        {
            var query = context.Products.AsQueryable();
            if (!string.IsNullOrWhiteSpace(brand))
            {
                query = query.Where(p => p.Brand == brand);
            }
            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(p => p.Type == type);
            }
            if (!string.IsNullOrWhiteSpace(sort))
            {
                query = sort.ToLower() switch
                {
                    "priceasc" => query.OrderBy(p => p.Price),
                    "pricedesc" => query.OrderByDescending(p => p.Price),
                    _ => query.OrderBy(p => p.Name)
                };
            }
            return await query.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await context.Products.FindAsync(id);
        }

        public void AddProductAsync(Product product)
        {
            context.Products.Add(product);
        }

        public void UpdateProductAsync(Product product)
        {
            context.Products.Update(product);
        }

        public void DeleteProductAsync(Product product)
        {
            context.Products.Remove(product);
        }

        public bool ProductExistsAsync(int id)
        {
            return context.Products.Any(e => e.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<IReadOnlyList<string>> GetBrandsAsync()
        {
            return await context.Products
                .Select(p => p.Brand)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<string>> GetTypesAsync()
        {
            return await context.Products
                .Select(p => p.Type)
                .Distinct()
                .ToListAsync();
        }
    }
}
