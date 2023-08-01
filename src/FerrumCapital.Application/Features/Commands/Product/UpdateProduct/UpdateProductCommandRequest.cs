using FerrumCapital.Application.Features.Commands.Product.CreateProduct;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerrumCapital.Application.Features.Commands.Product.UpdateProduct
{
    public record UpdateProductCommandRequest(int Id, IFormFile File) : IRequest<UpdateProductCommandResponse>;
}
