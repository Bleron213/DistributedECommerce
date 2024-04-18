using BoxCommerce.Warehouse.Application.Common.Infrastructure;
using BoxCommerce.Warehouse.Application.Services;
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

            var rnd = new Random();
            var productVWGolfGtI = new Product("VOLKSWAGEN_GOLF_GTI", rnd.Next(1, 100_000).ToString(), rnd.Next(1, 100_000).ToString());
            var turboInline20L = new Component("TURBO_INLINE_FOUR_2_0L", productVWGolfGtI.Id, rnd.Next(1, 100_000).ToString(), ComponentType.ENGINE);
            var backbone = new Component("BODY_ON_FRAME", productVWGolfGtI.Id, rnd.Next(1, 100_000).ToString(), ComponentType.CHASSIS);
            var performanceTrackPack = new Component("BODY_ON_FRAME", productVWGolfGtI.Id, rnd.Next(1, 100_000).ToString(), ComponentType.CHASSIS);

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
        }
    }
}
