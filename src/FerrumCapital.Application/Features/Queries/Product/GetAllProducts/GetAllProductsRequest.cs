using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerrumCapital.Application.Features.Queries.Product.GetAllProducts
{
    public class GetAllProductsRequest:IRequest<List<GetAllProductsResponse>>
    {
    }
}
