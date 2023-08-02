using FerrumCapital.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            try
            {
                var updatedProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id);

                if (updatedProduct == null)
                {
                    return new UpdateProductCommandResponse { IsSuccess = false, ErrorMessage = "Product not found." };
                }

                if (System.IO.File.Exists(updatedProduct.FilePath))
                {
                    System.IO.File.Delete(updatedProduct.FilePath);
                }

                const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB
                string[] AllowedMimeTypes = { "image/jpeg", "image/png", "application/pdf" };
                string AllowedFileNamePattern = @"^[A-Za-z0-9_-]+\.(jpg|jpeg|png|pdf)$";

                if (request.File.Length > MaxFileSizeBytes)
                {
                    return new UpdateProductCommandResponse { IsSuccess = false, ErrorMessage = "File size exceeds the limit." };
                }

                if (!AllowedMimeTypes.Contains(request.File.ContentType))
                {
                    return new UpdateProductCommandResponse { IsSuccess = false, ErrorMessage = "Invalid file type." };
                }

                if (!Regex.IsMatch(request.File.FileName, AllowedFileNamePattern))
                {
                    return new UpdateProductCommandResponse { IsSuccess = false, ErrorMessage = "Invalid file name." };
                }

                if (request.File == null || request.File.Length == 0)
                {
                    return new UpdateProductCommandResponse { IsSuccess = false, ErrorMessage = "File can not be null" };
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.File.FileName);

                //FerrumCapital.API\bin\Debug\net7.0\Downloaded\
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

                updatedProduct.Name = request.File.FileName;
                updatedProduct.FilePath = filePath;

                await _context.SaveChangesAsync(cancellationToken);

                return new UpdateProductCommandResponse
                {
                    IsSuccess = true,
                    FilePath = updatedProduct.FilePath,
                    Name = updatedProduct.Name,
                };
            }
            catch (Exception ex)
            {
                return new UpdateProductCommandResponse { IsSuccess = false, ErrorMessage = "An error occurred during file update." };
            }
        }
    }
}
