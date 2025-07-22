namespace CleanFoodVietAPI.Application.DTOs.ProductDTOs
{
    public record CreateProductDTO
    {
        //Product Data Field
        public string ProductName { get; set; } = null!;
        public string Status { get; set; } = null!;

        //Product Category Data Field
        public Ulid? ProductCategoryId { get; set; }
        //public string Name { get; set; } = null!;
        //public string? Description { get; set; }

        //Product Price Data Field
        public decimal Price { get; set; }
        public string Currency { get; set; } = null!;
        public DateTime AvailabledDate { get; set; }
        public bool IsCurrent { get; set; }
        public string WeightUnit { get; set; } = null!;
    }
}
