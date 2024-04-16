using BoxCommerce.Warehouse.Common.Request;
using BoxCommerce.Warehouse.Common.Response;

namespace BoxCommerce.Warehouse.Application.Stocks.Queries
{
    public class CheckInStockQuery : IRequest<List<InStockResponse>>
    {
        public CheckInStockQuery(VehicleInStockRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            Request = request;
        }

        public VehicleInStockRequest Request { get; }
    }

    public class CheckInStockQueryHandler : IRequestHandler<CheckInStockQuery, List<InStockResponse>>
    {
        public CheckInStockQueryHandler()
        {
        }

        public Task<List<InStockResponse>> Handle(CheckInStockQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
