using FerrumCapital.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerrumCapital.Application.Features.Commands.Product.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
    {
        private readonly IApplicationDbContext _context;

        public UpdateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            var updatedProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id);

            if (System.IO.File.Exists(updatedProduct.FilePath))
            {
                System.IO.File.Delete(updatedProduct.FilePath);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.File.FileName);
            var directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Downloaded");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = Path.Combine(directoryPath, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(fileStream);
            }

            updatedProduct.Name = request.File.Name;
            updatedProduct.FilePath = filePath;

            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateProductCommandResponse
            {
                FilePath = updatedProduct.FilePath,
                Name = updatedProduct.Name,
            };
        }
    }
}
