using BoxCommerce.Warehouse.Application.Common.Infrastructure;
using BoxCommerce.Warehouse.Domain.Entities;
using BoxCommerce.Warehouse.Domain.Enums;
using BoxCommerce.Warehouse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BoxCommerce.Warehouse.API.Services
{
    public class WarehouseSeeder
    {
        private readonly WarehouseDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public WarehouseSeeder(
            WarehouseDbContext context, 
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task SeedDefaultData()
        {
            // ---------- Simulate Product in READY state (no need to order additional components) --------------------------------- //

            //{
            //  "vehicleCode": "VOLKSWAGEN_GOLF_GTI",
            //  "components": [
            //    {
            //      "code": "TURBO_INLINE_FOUR_2_0L"
            //    },
            //    {
            //      "code": "BACKBONE"
            //    },
            //    {
            //      "code": "PERFORMANCE_TRACK_PACK"
            //    }
            //  ]
            //}

            var rnd = new Random();
            var productVWGolfGtI = new Product("VOLKSWAGEN_GOLF_GTI", rnd.Next(1, 100_000).ToString());
            var turboInline20L = new Component("TURBO_INLINE_FOUR_2_0L", productVWGolfGtI.Id);
            var backbone = new Component("BODY_ON_FRAME", productVWGolfGtI.Id);
            var performanceTrackPack = new Component("BODY_ON_FRAME", productVWGolfGtI.Id);

            _context.Products.Add(productVWGolfGtI);
            _context.Components.Add(turboInline20L);
            _context.Components.Add(backbone);
            _context.Components.Add(performanceTrackPack);

            _context.SaveChanges();

            _context.Products
                .Where(x => x.Id == productVWGolfGtI.Id)
                .ExecuteUpdate(setters => setters
                    .SetProperty(b => b.OrderNumber, (string?)null)
                    .SetProperty(b => b.Status, ProductStatus.CANCELLED));

            _context.Components
                .Where(x => x.ProductId == productVWGolfGtI.Id)
                .ExecuteUpdate(setters => setters
                    .SetProperty(b => b.Status, ComponentStatus.ASSEMBLED));

            // ------------------------------------------------------------------- // 

            // ----------- Simulate Product not in stock (but components in stock) ---------------------------- // 

            //{
            //    "vehicleCode": "BMW_M3",
            //      "components": [
            //        {
            //                        "code": "ELECTRIC_MOTOR_60_KWH"
            //        },
            //        {
            //                        "code": "TUBULAR_SPACE_FRAME"
            //        },
            //        {
            //                        "code": "CITY_URBAN_PACK"
            //        }
            //      ]
            //}


            var ELECTRIC_MOTOR_60_KWH = new Component("ELECTRIC_MOTOR_60_KWH");
            var TUBULAR_SPACE_FRAME = new Component("TUBULAR_SPACE_FRAME");
            var CITY_URBAN_PACK = new Component("CITY_URBAN_PACK");

            _context.Components.Add(ELECTRIC_MOTOR_60_KWH);
            _context.Components.Add(TUBULAR_SPACE_FRAME);
            _context.Components.Add(CITY_URBAN_PACK);

            _context.SaveChanges();
            _context.Components
                .Where(x => new List<Guid> { ELECTRIC_MOTOR_60_KWH.Id,TUBULAR_SPACE_FRAME.Id, CITY_URBAN_PACK.Id }.Contains(x.Id))
                .ExecuteUpdate(setters => setters
                    .SetProperty(b => b.Status, ComponentStatus.READY)                    
                    );

            // ------------------------------------------------------------------- // 
        }
    }
}
