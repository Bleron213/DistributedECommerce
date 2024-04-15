using BoxCommerce.Orders.Domain.Enums;

namespace BoxCommerce.Orders.Domain.Entities
{
    public class Component : BaseEntity
    {
        public Component(string name, string componentCode, ComponentType componentType)
        {
            Id = Guid.NewGuid();
            Name = name;
            ComponentCode = componentCode;
            ComponentType = componentType;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string ComponentCode { get; private set; }
        public ComponentType ComponentType { get; private set; }
    }
}
