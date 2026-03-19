using Backend.Core.Entities;
using Backend.Core.Interfaces;
using Backend.Core.Specification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IGenericRepository<Product> _productRepository) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
        {
            var spec = new ProductSpecification(brand, type,sort);

            var products = await _productRepository.ListAsync(spec);

            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _productRepository.Add(product);
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

            if (!_productRepository.EntityExists(id))
            {
                return NotFound();
            }

            _productRepository.Update(product);
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
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _productRepository.Delete(product);
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
            var spec = new BrandListSpecification();

            return Ok(await _productRepository.ListAsync(spec));
        }

        // New endpoint to get distinct types
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var spec = new TypeListSpecification();

            return Ok(await _productRepository.ListAsync(spec));
        }
    }
}
