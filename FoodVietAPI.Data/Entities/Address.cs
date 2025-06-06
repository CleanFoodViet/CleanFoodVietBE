using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Data.Entities
{
    public class Address
    {
        [Key]
        public Ulid AddressId { get; set; }
        public string AddessLine1 { get; set; } = null!;
        public string? AddessLine2 { get; set; }
        public string City { get; set; } = null!;
        public string Province { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Country { get; set; } = null!;
        public Ulid AccountId { get; set; }

        public virtual Account Account { get; set; } = null!;
    }
}
