using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Data.Entities
{
    public class Account
    {
        [Key]
        public Ulid AccountId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string Avatar { get; set; } = null!;
        public string Status { get; set; } = null!;
        public bool IsVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Ulid RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Address> Addresses { get; set; } = new HashSet<Address>();
        public virtual ICollection<Appointment> GardenerAppointments { get; set; } = new HashSet<Appointment>();
        public virtual ICollection<Appointment> RetailerAppointments { get; set; } = new HashSet<Appointment>();
        public virtual ICollection<Cart> RetailerCarts { get; set; } = new HashSet<Cart>();
        public virtual ICollection<Cart> GardenerCarts { get; set; } = new HashSet<Cart>();
        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
        public virtual ICollection<Certificate> Certificates { get; set; } = new HashSet<Certificate>();
        public virtual ICollection<ChatMessage> SenderMessages { get; set; } = new HashSet<ChatMessage>();
        public virtual ICollection<ChatMessage> ReceiverMessages { get; set; } = new HashSet<ChatMessage>();
        public virtual ICollection<Favorite> Favorites { get; set; } = new HashSet<Favorite>();
        public virtual ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public virtual ICollection<Report> Reports { get; set; } = new HashSet<Report>();
        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public virtual ICollection<Order> RetailerOrders { get; set; } = new HashSet<Order>();
        public virtual ICollection<Order> GardenerOrders { get; set; } = new HashSet<Order>();
        public virtual ICollection<SubscriptionContract> ServicePackageContracts { get; set; } = new HashSet<SubscriptionContract>();
        public virtual ICollection<ServicePackageOrder> ServicePackageOrders { get; set; } = new HashSet<ServicePackageOrder>();
        public virtual ICollection<GardenerIncome> GardenerIncomes { get; set; } = new HashSet<GardenerIncome>();
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
        public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new HashSet<ProductCategory>();
    }
}
