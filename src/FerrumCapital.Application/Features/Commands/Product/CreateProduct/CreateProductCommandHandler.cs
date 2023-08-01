using FerrumCapital.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerrumCapital.Application.Features.Commands.Product.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        private readonly IApplicationDbContext _context;
        //private readonly IWebHostEnvironment _environment;

        public CreateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
            //_environment = environment;
        }   

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.File == null || request.File.Length == 0)
                {
                    return new CreateProductCommandResponse { IsSuccess = false };
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.File.FileName);

                var directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Downloaded");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath); 
                }

                var filePath = Path.Combine(directoryPath, uniqueFileName);
                //var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Downloaded", uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.File.CopyToAsync(fileStream);
                }

                var product = new FerrumCapital.Domain.Entities.Product
                {
                    Name = request.File.FileName,
                    FilePath = filePath
                };
                _context.Products.Add(product);
                 await _context.SaveChangesAsync(cancellationToken);

                // Return the file URL in the response
                return new CreateProductCommandResponse { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new CreateProductCommandResponse { IsSuccess = false };
            }
        }
    }
}
