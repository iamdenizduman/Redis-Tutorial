using Microsoft.AspNetCore.Mvc;
using Redis_Example_Catalog.Models;
using Redis_Example_Catalog.Services;

namespace Redis_Example_Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product updatedProduct)
        {
            var existing = await _productService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            updatedProduct.Id = id;
            await _productService.UpdateAsync(updatedProduct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _productService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            await _productService.DeleteAsync(id);
            return NoContent();
        }
    }
}
