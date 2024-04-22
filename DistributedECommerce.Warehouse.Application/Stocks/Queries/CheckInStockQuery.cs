using DistributedECommerce.Warehouse.Application.Common.Infrastructure;
using DistributedECommerce.Warehouse.Common.Request;
using DistributedECommerce.Warehouse.Common.Response;
using DistributedECommerce.Warehouse.Domain.Entities;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using DistributedECommerce.Warehouse.Domain.Enums;

namespace DistributedECommerce.Warehouse.Application.Stocks.Queries
{
    public class CheckInStockQuery : IRequest<InStockResponse>
    {
        public CheckInStockQuery(InStockRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            Payload = request;
        }

        public InStockRequest Payload { get; }
    }

    public class CheckInStockQueryHandler : IRequestHandler<CheckInStockQuery, InStockResponse>
    {
        private readonly IWarehouseDbContext _warehouseDbContext;

        public CheckInStockQueryHandler(
            IWarehouseDbContext warehouseDbContext
            )
        {
            _warehouseDbContext = warehouseDbContext;
        }

        public async Task<InStockResponse> Handle(CheckInStockQuery request, CancellationToken cancellationToken)
        {
            var dbConnectionString = _warehouseDbContext.Database.GetConnectionString();

            using IDbConnection db = new SqlConnection(dbConnectionString);

            // First case is when a vehicle has status CANCELLED and all its components are ASSEMBLED
            // This means that this is a pre-assembled vehicle that was returned from the client. It is assumed to not have any defect
            var product = await db.QueryFirstOrDefaultAsync<Domain.Entities.Product>(@"
                select p.*
                from Products p
                left join Components c on c.ProductId = p.Id
                where p.Code = @ProductCode 
		                and p.OrderNumber is null 
		                and p.Status = @ProductStatus
		                and c.Status = @ComponentStatus;
                ", new { ProductCode = request.Payload.ProductCode, ProductStatus = ProductStatus.ASSEMBLED, ComponentStatus = ComponentStatus.ASSEMBLED});

            if(product != null)
            {
                return new InStockResponse()
                {
                    ProductId = product.Id.ToString(),
                    Code = product.Code,
                    Status = InStockResponse.StockStatus.IN_STOCK,
                };
            }

            return new InStockResponse()
            {
                Code = request.Payload.ProductCode,
                Status = InStockResponse.StockStatus.NOT_IN_STOCK,
            };
        }
    }
}
