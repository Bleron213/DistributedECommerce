namespace BoxCommerce.Orders.Domain.Entities
{
    public class OrderProduct : BaseEntity
    {
        public OrderProduct()
        {
        }

        public OrderProduct(string productId)
        {
            ProductId = productId;
        }

        /// <summary>
        /// Product Id in the Warehouse
        /// </summary>
        public string ProductId { get; set; }
        public Guid OrderId { get; set; }
        #region Navigation
        public Order Order { get; set; }
        #endregion
    }
}
