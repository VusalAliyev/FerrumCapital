using FerrumCapital.Application.Features.Commands.Product.CreateProduct;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FerrumCapital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromQuery] CreateProductCommandRequest createProductCommandRequest)
        {
            var response=await _mediator.Send(createProductCommandRequest);
            return Ok(response);
        }
    }
}
