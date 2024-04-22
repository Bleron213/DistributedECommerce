using DistributedECommerce.Orders.Domain.Entities;
using DistributedECommerce.Utils.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DistributedECommerce.Orders.UnitTests
{
    public class OrderTests
    {
        [Fact]
        public void Order()
        {
            #region Arrange
            Order order = new Order();
            #endregion

            #region Act

            var act = () => order.DeliverOrder();

            #endregion

            #region Assert
            var ex = Assert.Throws<AppException>(act);
            Assert.Equal(HttpStatusCode.BadRequest, ex.Error.StatusCode);
            Assert.Equal("invalid_status_move", ex.Error.ErrorKey);
            Assert.Equal($"Invalid status move. Cannot move from IN_PROCESS to DELIVERED", ex.Error.Message);
            #endregion

        }
    }
}
