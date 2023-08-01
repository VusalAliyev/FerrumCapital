using FerrumCapital.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerrumCapital.Application.Features.Queries.Product.GetAllProducts
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsRequest, List<GetAllProductsResponse>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllProductsHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetAllProductsResponse>> Handle(GetAllProductsRequest request, CancellationToken cancellationToken)
        {
            return await _context.Products.Select(p => new GetAllProductsResponse
            {
                FilePath = p.FilePath,
                Name = p.Name
            }).ToListAsync();
        }
    }
}
