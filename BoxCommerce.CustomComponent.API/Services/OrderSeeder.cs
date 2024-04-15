using BoxCommerce.Orders.Application.Common.Infrastructure;
using BoxCommerce.Orders.Infrastructure.Data;

namespace BoxCommerce.CustomComponent.API.Services
{
    public class OrderSeeder
    {
        private readonly BoxCommerceOrderDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public OrderSeeder(BoxCommerceOrderDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task SeedDefaultData()
        {
            if (!_context.Orders.Any(x => x.OrderNumber == "36309"))
            {
                _context.Orders.Add(new Domain.Entities.Order("36309", Guid.Parse(_currentUserService.UserId)));
            }
            _context.SaveChanges();
        }
    }
}
