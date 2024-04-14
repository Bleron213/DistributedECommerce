using BoxCommerce.Orders.Application.Common.Infrastructure;

namespace BoxCommerce.Orders.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        /// <summary>
        /// Purposely mocked for demo purposes.
        /// In a real word scenario, this comes from HttpContext
        /// </summary>
        public string UserId => "6efc7698-7115-468d-9143-db4dd2c429c5";
        public string Username => "BleronQorri";
    }
}
