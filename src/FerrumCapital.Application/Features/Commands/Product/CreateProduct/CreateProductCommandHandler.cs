using FerrumCapital.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FerrumCapital.Application.Features.Commands.Product.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        // Constants for security measures
        private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB
        private static readonly string[] AllowedMimeTypes = { "image/jpeg", "image/png", "application/pdf" };
        private const string AllowedFileNamePattern = @"^[A-Za-z0-9_-]+\.(jpg|jpeg|png|pdf)$"; // Example: Accepts filenames with letters, numbers, underscores, and hyphens, ending with .jpg, .jpeg, .png, or .pdf

        private readonly IApplicationDbContext _context;

        public CreateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }   

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Security measures
                if (request.File.Length > MaxFileSizeBytes)
                {
                    return new CreateProductCommandResponse { IsSuccess = false, ErrorMessage = "File size exceeds the limit." };
                }

                if (!AllowedMimeTypes.Contains(request.File.ContentType))
                {
                    return new CreateProductCommandResponse { IsSuccess = false, ErrorMessage = "Invalid file type." };
                }

                if (!Regex.IsMatch(request.File.FileName, AllowedFileNamePattern))
                {
                    return new CreateProductCommandResponse { IsSuccess = false, ErrorMessage = "Invalid file name." };
                }

                if (request.File == null || request.File.Length == 0)
                {
                    return new CreateProductCommandResponse { IsSuccess = false, ErrorMessage="File can not be null" };
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
                return new CreateProductCommandResponse { IsSuccess = false,ErrorMessage= "An error occurred during file upload." };
            }
        }
    }
}
