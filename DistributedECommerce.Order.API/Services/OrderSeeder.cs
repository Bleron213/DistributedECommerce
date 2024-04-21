using BoxCommerce.Orders.Application.Common.Infrastructure;
using BoxCommerce.Orders.Domain.Enums;
using BoxCommerce.Orders.Infrastructure.Data;
using System.Reflection;
using System.Threading;

namespace BoxCommerce.Orders.API.Services
{
    public class OrderSeeder
    {
        private readonly OrderDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public OrderSeeder(OrderDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task SeedDefaultData()
        {
            _context.SaveChanges();
        }
    }
}
