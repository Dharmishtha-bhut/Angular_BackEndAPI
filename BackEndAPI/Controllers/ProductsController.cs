using Backend.Core.Entities;
using Backend.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext storeContext;

        public ProductsController(StoreContext context)
        {
            this.storeContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await storeContext.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await storeContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            storeContext.Products.Add(product);
            await storeContext.SaveChangesAsync();
            return product;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id,Product product)
        {
            if (product.Id != id && !ProductExists(product.Id))
            {
                return NotFound();
            }

            storeContext.Entry(product).State = EntityState.Modified;
            await storeContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await storeContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            storeContext.Products.Remove(product);
            await storeContext.SaveChangesAsync();
            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return storeContext.Products.Any(e => e.Id == id);
        }
    }
}
