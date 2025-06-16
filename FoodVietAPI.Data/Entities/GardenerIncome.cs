using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Data.Entities
{
    public class GardenerIncome
    {
        [Key]
        public Ulid GardenerIncomeId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = null!;
        public DateTime TransactionDate { get; set; }
        public Ulid GardenerId { get; set; }

        public virtual Account Account { get; set; } = null!;
    }
}
