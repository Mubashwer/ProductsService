using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Products.API.Application.Dtos;
using Products.API.Infrastructure.Services.ProductService;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Products.API.Controllers
{
    [Route("products")]
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
        [ProducesDefaultResponseType(typeof(ProblemDetails))]
        public async Task<PagedListDto<ProductDto>> Get(
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Range(1, int.MaxValue)] int pageSize = 100)
        {
            var products = await _productService.GetPagedAsync(pageNumber, pageSize);
            return new PagedListDto<ProductDto>(products);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesDefaultResponseType(typeof(ProblemDetails))]
        public async Task<ActionResult<ProductDto>> Get([FromRoute] Guid id)
        {
            var productDto = await _productService.FindByIdAsync(id);
            if (productDto is null)
            {
                return NotFound();
            }

            return productDto;
        }

        [HttpPost]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesDefaultResponseType(typeof(ProblemDetails))]
        public async Task<ActionResult<ProductDto>> Post([FromBody] ProductDto productDto)
        {
            var createdProductDto = await _productService.AddAsync(productDto);
            var uri = UriHelper.BuildRelative(path: Path.Join(Request.Path, createdProductDto.Id.ToString()));

            return Created(uri, createdProductDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesDefaultResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] ProductDto productDto)
        {
            if (await _productService.FindByIdAsync(id) is null)
            {
                return NotFound();
            }

            productDto.Id = id;
            await _productService.UpdateAsync(productDto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesDefaultResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var productDto = await _productService.FindByIdAsync(id);
            if (productDto is null)
            {
                return NotFound();
            }

            await _productService.DeleteAsync(productDto);
            return NoContent();
        }
    }
}