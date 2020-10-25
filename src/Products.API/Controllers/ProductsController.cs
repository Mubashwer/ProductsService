using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Products.API.Application.Dtos;
using Products.API.Extensions;
using Products.API.Infrastructure.Services.ProductService;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Products.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<PagedListDto<ProductDto>> Get(
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Range(1, int.MaxValue)] int pageSize = 100)
        {
            var productDtos = await _productService.GetPagedProductsAsync(pageNumber, pageSize);
            return productDtos.ToPagedListDto();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult<ProductDto>> Get([FromRoute] Guid id)
        {
            var productDto = await _productService.FindProductByIdAsync(id);
            if (productDto is null)
            {
                return NotFound();
            }

            return productDto;
        }

        [HttpPost]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<ActionResult<ProductDto>> Post([FromBody] ProductDto productDto)
        {
            var createdProductDto = await _productService.AddProductAsync(productDto);

            return CreatedAtAction(nameof(Get), new {id = createdProductDto.Id}, createdProductDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] ProductDto productDto)
        {
            productDto.Id = id;

            var result = await _productService.UpdateProductAsync(productDto);

            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _productService.DeleteProductAsync(id);

            if (!result) return NotFound();
            return NoContent();
        }
    }
}