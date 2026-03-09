using ECommerceOrdersAPI.Models;
using ECommerceOrdersAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceOrdersAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var products = await _productService.GetProducts();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductsById(int id)
        {
            var product = await _productService.GetProductById(id);

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] AddProductDto dto)
        {
            var id = await _productService.AddProduct(dto);

            return CreatedAtAction(nameof(GetProductsById), new { id }, null);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditProduct(int id, [FromBody] EditProductDto dto)
        {
            await _productService.EditProduct(id, dto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProduct(id);

            return NoContent();
        }


    }
}
