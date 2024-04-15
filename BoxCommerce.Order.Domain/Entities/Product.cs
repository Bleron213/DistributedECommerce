namespace BoxCommerce.Orders.Domain.Entities
{
    public class Product: BaseEntity
    {
        public Product(string name, string code)
        {
            Id = Guid.NewGuid();
            Name = name;
            Code = code;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Code { get; private set; }
    }
}
