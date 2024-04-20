using BoxCommerce.Warehouse.Application.Common.Infrastructure;
using BoxCommerce.Warehouse.Common.Request;
using BoxCommerce.Warehouse.Common.Response;
using BoxCommerce.Warehouse.Domain.Entities;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using BoxCommerce.Warehouse.Domain.Enums;

namespace BoxCommerce.Warehouse.Application.Stocks.Queries
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
            var product = await db.QueryFirstOrDefaultAsync<Product>(@"
                select * from Products p
                where p.Code = @ProductCode and p.OrderNumber is null and p.Status = @ProductStatus
                    AND NOT EXISTS (
                        SELECT 1 
                        FROM Components c
                        WHERE c.ProductID = p.Id
                            AND c.Status <> @ComponentStatus -- All components must have this status
                    );
                ", new { ProductCode = request.Payload.ProductCode, ProductStatus = ProductStatus.CANCELLED, ComponentStatus = ComponentStatus.ASSEMBLED });

            if(product != null)
            {
                return new InStockResponse()
                {
                    ProductId = product.Id.ToString(),
                    Code = product.Code,
                    Status = InStockResponse.StockStatus.IN_STOCK,
                };
            }

            return new InStockResponse();
        }
    }
}
