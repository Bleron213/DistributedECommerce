using DistributedECommerce.Orders.Application.Common.Infrastructure;
using DistributedECommerce.Orders.Common.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedECommerce.Orders.Application.Products.Queries
{
    public class GetAvailableProductsQuery : IRequest<List<ProductResponse>>
    {
    }

    public class GetAvailableProductsQueryHandler : IRequestHandler<GetAvailableProductsQuery, List<ProductResponse>>
    {
        private readonly IOrderDbContext _context;

        public GetAvailableProductsQueryHandler(IOrderDbContext context)
        {
            _context = context;
        }

        public Task<List<ProductResponse>> Handle(GetAvailableProductsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
