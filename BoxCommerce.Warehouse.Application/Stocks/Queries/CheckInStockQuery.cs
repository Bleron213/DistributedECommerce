using BoxCommerce.Warehouse.Application.Common.Application.Services;
using BoxCommerce.Warehouse.Application.Common.Infrastructure;
using BoxCommerce.Warehouse.Application.Services;
using BoxCommerce.Warehouse.Common.Request;
using BoxCommerce.Warehouse.Common.Response;
using BoxCommerce.Warehouse.Domain.Entities;

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
        private readonly IWarehouseDbContext _warehouseDbContext;
        private readonly IComponentHashingService _componentHashingService;

        public CheckInStockQueryHandler(
            IWarehouseDbContext warehouseDbContext,
            IComponentHashingService componentHashingService
            )
        {
            _warehouseDbContext = warehouseDbContext;
            _componentHashingService = componentHashingService;
        }

        public async Task<List<InStockResponse>> Handle(CheckInStockQuery request, CancellationToken cancellationToken)
        {
            var productCode = request.Request.ProductCode;
            var components = request.Request.CustomComponents;

            // Check if a Pre-assembled vehicle is ready for shipping
            var componentsHash = _componentHashingService.HashComponentCodes(request.Request.ProductCode, request.Request.CustomComponents.Select(x => x.Code).ToList());
            var productAvailable = await _warehouseDbContext.Products.AnyAsync(x => 
                x.Code == productCode && 
                x.PropertiesHash == componentsHash && 
                (x.Status == Domain.Enums.ProductStatus.ASSEMBLED || x.Status == Domain.Enums.ProductStatus.CANCELLED));

            if (productAvailable)
            {
                return new List<InStockResponse>
                {

                };
            }


        }
    }
}
