using FerrumCapital.Application.Common.Interfaces;
using MediatR;
using MediatR.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerrumCapital.Application.Features.Commands.Product.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommandRequest, DeleteProductCommandResponse>
    {
        private readonly IApplicationDbContext _context;
        public DeleteProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

       

        public async Task<DeleteProductCommandResponse> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
        {
            var deletedProduct = _context.Products.FirstOrDefault(p => p.Id == request.Id);
            _context.Products.Remove(deletedProduct);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteProductCommandResponse { IsSuccess = true };
        }
    }
}
