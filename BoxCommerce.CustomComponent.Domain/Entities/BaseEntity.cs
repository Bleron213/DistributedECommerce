using System.ComponentModel.DataAnnotations;

namespace BoxCommerce.CustomComponent.Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }
        public DateTimeOffset? LastModifiedOn { get; set; }
        [MaxLength(50)]
        public string? LastModifiedBy { get; set; }
    }
}
