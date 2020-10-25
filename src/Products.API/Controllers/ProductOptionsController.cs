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
    [Route("api/products/{productId}/options")]
    [ApiController]
    public class ProductOptionsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductOptionsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult<PagedListDto<ProductOptionDto>>> Get(
            [FromRoute] Guid productId,
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Range(1, int.MaxValue)] int pageSize = 100)
        {
            var productOptionDtos = await _productService.GetPagedProductOptionsAsync(productId, pageNumber, pageSize);
            if (productOptionDtos is null)
            {
                return NotFound();
            }

            return productOptionDtos.ToPagedListDto();
        }

        [HttpGet("{productOptionId}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult<ProductOptionDto>> Get(
            [FromRoute] Guid productId,
            [FromRoute] Guid productOptionId)
        {
            var productOptionDto = await _productService.FindProductOptionByIdAsync(productId, productOptionId);
            if (productOptionDto is null)
            {
                return NotFound();
            }

            return productOptionDto;
        }

        [HttpPost]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult<ProductDto>> Post(
            [FromRoute] Guid productId,
            [FromBody] ProductOptionDto productOptionDto)
        {
            var createdProductOptionDto = await _productService.AddProductOptionAsync(productId, productOptionDto);
            if (createdProductOptionDto is null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(Get),
                new {productId, productOptionId = createdProductOptionDto.Id},
                createdProductOptionDto);
        }

        [HttpPut("{productOptionId}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<IActionResult> Put(
            [FromRoute] Guid productId,
            [FromRoute] Guid productOptionId,
            [FromBody] ProductOptionDto productOptionDto)
        {
            productOptionDto.Id = productOptionId;

            var result = await _productService.UpdateProductOptionAsync(productId, productOptionDto);

            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{productOptionId}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid productId, [FromRoute] Guid productOptionId)
        {
            var result = await _productService.DeleteProductOptionAsync(productId, productOptionId);

            if (!result) return NotFound();
            return NoContent();
        }
    }
}