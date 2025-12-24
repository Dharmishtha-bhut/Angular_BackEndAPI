using Backend.Core.Entities;
using Backend.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductRepository _productRepository) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
        {
            var products = await _productRepository.GetProductsAsync(brand,type,sort);
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _productRepository.AddProductAsync(product);
            var success = await _productRepository.SaveChangesAsync();
            if (!success)
            {
                return BadRequest("Failed to create product");
            }
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("Product ID mismatch");
            }

            if (!_productRepository.ProductExistsAsync(id))
            {
                return NotFound();
            }

            _productRepository.UpdateProductAsync(product);
            var success = await _productRepository.SaveChangesAsync();
            if (!success)
            {
                return BadRequest("Failed to update product");
            }
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _productRepository.DeleteProductAsync(product);
            var success = await _productRepository.SaveChangesAsync();
            if (!success)
            {
                return BadRequest("Failed to delete product");
            }
            return NoContent();
        }

        // New endpoint to get distinct brands
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var brands = await _productRepository.GetBrandsAsync();
            return Ok(brands);
        }

        // New endpoint to get distinct types
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var types = await _productRepository.GetTypesAsync();
            return Ok(types);
        }
    }
}
