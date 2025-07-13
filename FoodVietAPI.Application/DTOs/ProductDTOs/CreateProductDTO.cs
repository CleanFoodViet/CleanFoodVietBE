namespace CleanFoodVietAPI.Application.DTOs.ProductDTOs
{
    public record CreateProductDTO
    {
        //Product Data Field
        //public Ulid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        //public string? ImageUrl { get; set; } ~~~
        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
        public string Status { get; set; } = null!;
        //public Ulid ProductCategoryId { get; set; } <<<<
        //public Ulid GardenerId { get; set; } <<<<

        //Product Category Data Field
        public Ulid? ProductCategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        //public Ulid GardenerId { get; set; } <<<<

        //Product Price Data Field
        //public Ulid ProductPriceId { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; } = null!;
        public DateTime AvailabledDate { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
        public bool IsCurrent { get; set; }
        public string WeightUnit { get; set; } = null!;
        //public Ulid Productd { get; set; } <<<<
    }
}
