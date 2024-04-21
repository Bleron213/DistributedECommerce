using DistributedECommerce.Orders.Application.Common.Infrastructure;
using DistributedECommerce.Orders.Domain.Enums;
using DistributedECommerce.Orders.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading;

namespace DistributedECommerce.Orders.API.Services
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
            _context.OrderProducts.ExecuteDelete();
            _context.Orders.ExecuteDelete();
            _context.SaveChanges();
        }
    }
}
