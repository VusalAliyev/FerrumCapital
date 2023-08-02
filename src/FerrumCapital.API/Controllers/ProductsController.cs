using FerrumCapital.Application.Features.Commands.Product.CreateProduct;
using FerrumCapital.Application.Features.Commands.Product.DeleteProduct;
using FerrumCapital.Application.Features.Commands.Product.UpdateProduct;
using FerrumCapital.Application.Features.Queries.Product.GetAllProducts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FerrumCapital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var response = await _mediator.Send(new GetAllProductsRequest());
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromQuery] CreateProductCommandRequest createProductCommandRequest)
        {
            var response=await _mediator.Send(createProductCommandRequest);
            return Ok(response);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct([FromQuery]DeleteProductCommandRequest deleteProductCommandRequest)
        {
            var response = await _mediator.Send(deleteProductCommandRequest);
            return Ok(response);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromQuery] UpdateProductCommandRequest updateProductCommandRequest)
        {
            var response = await _mediator.Send(updateProductCommandRequest);
            return Ok(response);
        }
    }
}
