using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerrumCapital.Application.Features.Commands.Product.CreateProduct
{
    public record CreateProductCommandRequest (IFormFile File) : IRequest<CreateProductCommandResponse>;

}
