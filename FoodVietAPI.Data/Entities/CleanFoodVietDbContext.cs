using Microsoft.EntityFrameworkCore;

namespace CleanFoodVietAPI.Data.Entities
{
    public partial class CleanFoodVietDbContext : DbContext
    {
        public CleanFoodVietDbContext()
        {
        }

        public CleanFoodVietDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Address> Addesses { get; set; } = null!;
        public virtual DbSet<Appointment> Appointments { get; set; } = null!;
        public virtual DbSet<Cart> Carts { get; set; } = null!;
        public virtual DbSet<CartItem> CartItems { get; set; } = null!;
        public virtual DbSet<Certificate> Certificates { get; set; } = null!;
        public virtual DbSet<ChatMessage> ChatMessages { get; set; } = null!;
        public virtual DbSet<ProductCategory> CleanFoodCategories { get; set; } = null!;
        public virtual DbSet<Favorite> Favorites { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<PostMedia> PostMedias { get; set; } = null!;
        public virtual DbSet<Report> Reports { get; set; } = null!;
        public virtual DbSet<Review> Reviews { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<ServiceFeature> ServiceFeatures { get; set; } = null!;
        public virtual DbSet<ServicePackage> ServicePackages { get; set; } = null!;
        public virtual DbSet<SubscriptionContract> SubscriptionContracts { get; set; } = null!;
        public virtual DbSet<PackageServiceFeature> PackageServiceFeatures { get; set; } = null!;
        public virtual DbSet<ServicePackageOrder> ServicePackageOrders { get; set; } = null!;
        public virtual DbSet<ServicePackageOrderPayment> ServicePackageOrderPayments { get; set; } = null!;
        public virtual DbSet<GardenerIncome> GardenerIncomes { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductPrice> ProductPrices { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");
                entity.HasKey(e => e.AccountId).HasName("PK_Account");

                entity.Property(e => e.AccountId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(),str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(11);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Gender).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Avatar).IsRequired();
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
                entity.Property(e => e.RoleId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();

                entity.HasIndex(e => e.Email).IsUnique()
                    .HasDatabaseName("IX_Account_Email");
                entity.HasIndex(e => e.PhoneNumber).IsUnique()
                    .HasDatabaseName("IX_Account_PhoneNumber");
                entity.HasIndex(e => e.RoleId).HasDatabaseName("IX_Account_RoleId");

                entity.HasOne(e => e.Role)
                      .WithMany(e => e.Accounts)
                      .HasForeignKey(e => e.RoleId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Account_Role");
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");
                entity.HasKey(e => e.AddressId).HasName("PK_Address");

                entity.Property(e => e.AddressId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(),str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.AddressLine1).IsRequired().HasMaxLength(255);
                entity.Property(e => e.AddressLine2).HasMaxLength(255);
                entity.Property(e => e.City).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Province).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PostalCode).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Country).IsRequired().HasMaxLength(50);
                entity.Property(e => e.AccountId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();

                entity.HasIndex(e => e.AccountId).HasDatabaseName("IX_Address_UserId");

                entity.HasOne(e => e.Account)
                      .WithMany(e => e.Addresses)
                      .HasForeignKey(e => e.AccountId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_Address_Account");
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("Appointment");
                entity.HasKey(e => e.AppointmentId).HasName("PK_Appointment");

                entity.Property(e => e.AppointmentId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(),str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.GardenerId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.RetailerId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.AppointmentDate).HasColumnType("datetime");
                entity.Property(e => e.Duration).IsRequired();
                entity.Property(e => e.Subject).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Location).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.AppointmentType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasColumnType("text");
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
                entity.Property(e => e.CancelledBy).HasMaxLength(10);
                entity.Property(e => e.CancellationReason).HasMaxLength(255);

                entity.HasIndex(e => e.GardenerId).HasDatabaseName("IX_Appointment_GardenerId");
                entity.HasIndex(e => e.RetailerId).HasDatabaseName("IX_Appointment_RetailerId");

                entity.HasOne(e => e.Gardener)
                      .WithMany(e => e.GardenerAppointments)
                      .HasForeignKey(e => e.GardenerId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Appointment_Gardeners");

                entity.HasOne(e => e.Retailer)
                      .WithMany(e => e.RetailerAppointments)
                      .HasForeignKey(e => e.RetailerId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Appointment_Retailers");
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Cart");
                entity.HasKey(e => e.CartId).HasName("PK_Cart");

                entity.Property(e => e.CartId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(),str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.RetailerId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.GardenerId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasIndex(e => e.RetailerId).HasDatabaseName("IX_Cart_RetailerId");
                entity.HasIndex(e => e.GardenerId).HasDatabaseName("IX_Cart_GardenerId");

                entity.HasOne(e => e.Retailer)
                      .WithMany(e => e.RetailerCarts)
                      .HasForeignKey(e => e.RetailerId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Cart_Retailers");

                entity.HasOne(e => e.Gardener)
                      .WithMany(e => e.GardenerCarts)
                      .HasForeignKey(e => e.GardenerId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Cart_Gardeners");
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.ToTable("CartItem");
                entity.HasKey(e => e.CartItemId).HasName("PK_CartItem");

                entity.Property(e => e.CartItemId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(),str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.CartId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.ProductId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.Price).HasColumnType("decimal(14,2)");
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.ProductUnit).IsRequired().HasMaxLength(10);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasIndex(e => e.CartId).HasDatabaseName("IX_CartItem_CartId");
                entity.HasIndex(e => e.ProductId).HasDatabaseName("IX_CartItem_PostId");

                entity.HasOne(e => e.Cart)
                      .WithMany(e => e.CartItems)
                      .HasForeignKey(e => e.CartId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_CartItem_Cart");

                entity.HasOne(e => e.Product)
                      .WithMany(e => e.CartItems)
                      .HasForeignKey(e => e.ProductId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_CartItem_Product");
            });

            modelBuilder.Entity<Certificate>(entity =>
            {
                entity.ToTable("Certificate");
                entity.HasKey(e => e.CertificateId).HasName("PK_Certificate");

                entity.Property(e => e.CertificateId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(),str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.ImageUrl).IsRequired();
                entity.Property(e => e.IssuingAuthority).IsRequired().HasMaxLength(255);
                entity.Property(e => e.IssueDate).HasColumnType("datetime");
                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.GardenerId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();

                entity.HasIndex(e => e.GardenerId).HasDatabaseName("IX_Certificate_GardenerId");

                entity.HasOne(e => e.Account)
                      .WithMany(e => e.Certificates)
                      .HasForeignKey(e => e.GardenerId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_Certificate_Account");
            });

            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.ToTable("ChatMessage");
                entity.HasKey(e => e.ChatMessageId).HasName("PK_ChatMessage");

                entity.Property(e => e.ChatMessageId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(),str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.SenderId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.ReceiverId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.MessageText).HasColumnType("text");
                entity.Property(e => e.SentAt).HasColumnType("datetime");
                entity.Property(e => e.MessageStatus).IsRequired().HasMaxLength(20);

                entity.HasIndex(e => e.SenderId).HasDatabaseName("IX_ChatMessage_SenderId");
                entity.HasIndex(e => e.ReceiverId).HasDatabaseName("IX_ChatMessage_ReceiverId");

                entity.HasOne(e => e.Receiver)
                      .WithMany(e => e.ReceiverMessages)
                      .HasForeignKey(e => e.ReceiverId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_ChatMessage_Receiver");

                entity.HasOne(e => e.Sender)
                      .WithMany(e => e.SenderMessages)
                      .HasForeignKey(e => e.SenderId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_ChatMessage_Sender");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.ToTable("ProductCategory");
                entity.HasKey(e => e.ProductCategoryId).HasName("PK_ProductCategory");

                entity.Property(e => e.ProductCategoryId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Description).HasColumnType("text");
            });

            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.ToTable("Favorite");
                entity.HasKey(e => e.FavoriteId).HasName("PK_Favorite");

                entity.Property(e => e.FavoriteId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.RetailerId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.PostId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.HasIndex(e => e.RetailerId).HasDatabaseName("IX_Favorite_RetailerId");
                entity.HasIndex(e => e.PostId).HasDatabaseName("IX_Favorite_PostId");

                entity.HasOne(e => e.Account)
                      .WithMany(e => e.Favorites)
                      .HasForeignKey(e => e.RetailerId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_Favorite_Account");

                entity.HasOne(e => e.Post)
                      .WithMany(e => e.Favorites)
                      .HasForeignKey(e => e.PostId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Favorite_Post");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");
                entity.HasKey(e => e.NotificationId).HasName("PK_Notification");

                entity.Property(e => e.NotificationId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.AccountId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.Message).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Link).HasMaxLength(255);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.HasIndex(e => e.AccountId).HasDatabaseName("IX_Notification_AccountId");

                entity.HasOne(e => e.Account)
                      .WithMany(e => e.Notifications)
                      .HasForeignKey(e => e.AccountId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_Notification_Account");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");
                entity.HasKey(e => e.OrderId).HasName("PK_Order");

                entity.Property(e => e.OrderId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.RetailerId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.GardenerId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(14,2)");
                entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(20);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.HasIndex(e => e.RetailerId).HasDatabaseName("IX_Order_RetailerId");
                entity.HasIndex(e => e.GardenerId).HasDatabaseName("IX_Order_GardenerId");

                entity.HasOne(e => e.Retailer)
                      .WithMany(e => e.RetailerOrders)
                      .HasForeignKey(e => e.RetailerId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Order_Retailers");

                entity.HasOne(e => e.Gardener)
                      .WithMany(e => e.GardenerOrders)
                      .HasForeignKey(e => e.GardenerId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Order_Gardeners");
            });

            modelBuilder.Entity<OrderDelivery>(entity =>
            {
                entity.ToTable("OrderDelivery");
                entity.HasKey(e => e.OrderDeliveryId).HasName("PK_OrderDelivery");

                entity.Property(e => e.OrderDeliveryId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.OrderId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.DeliveryDate).HasColumnType("datetime");
                entity.Property(e => e.DeliveryStatus).IsRequired().HasMaxLength(30);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasIndex(e => e.OrderId).HasDatabaseName("IX_OrderDelivery_OrderId");

                entity.HasOne(e => e.Order)
                      .WithMany(e => e.OrderDeliveries)
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_OrderDelivery_Order");
            });

            modelBuilder.Entity<OrderDeliveryItem>(entity =>
            {
                entity.ToTable("OrderDeliveryItem");
                entity.HasKey(e => e.OrderDeliveryItemId).HasName("PK_OrderDeliveryItem");

                entity.Property(e => e.OrderDeliveryItemId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.OrderDeliveryId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.OrderItemId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.DeliveredAt).HasColumnType("datetime");

                entity.HasIndex(e => e.OrderDeliveryId).HasDatabaseName("IX_OrderDeliveryItem_OrderDeliveryId");
                entity.HasIndex(e => e.OrderItemId).HasDatabaseName("IX_OrderDeliveryItem_OrderItemId");

                entity.HasOne(e => e.OrderDelivery)
                      .WithMany(e => e.OrderDeliveryItems)
                      .HasForeignKey(e => e.OrderDeliveryId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_OrderDeliveryItem_OrderDelivery");

                entity.HasOne(e => e.OrderItem)
                      .WithMany(e => e.OrderDeliveryItems)
                      .HasForeignKey(e => e.OrderItemId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_OrderDeliveryItem_OrderItem");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItem");
                entity.HasKey(e => e.OrderItemId).HasName("PK_OrderItem");

                entity.Property(e => e.OrderItemId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.OrderId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.ProductId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.Price).HasColumnType("decimal(14,2)");
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.ProductUnit).IsRequired().HasMaxLength(10);
                entity.Property(e => e.DeliveryStatus).IsRequired().HasMaxLength(25);

                entity.HasIndex(e => e.OrderId).HasDatabaseName("IX_OrderItem_OrderId");
                entity.HasIndex(e => e.ProductId).HasDatabaseName("IX_OrderItem_ProductId");

                entity.HasOne(e => e.Order)
                      .WithMany(e => e.OrderItems)
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_OrderItem_Order");

                entity.HasOne(e => e.Product)
                      .WithMany(e => e.OrderItems)
                      .HasForeignKey(e => e.ProductId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_OrderItem_Product");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");
                entity.HasKey(e => e.PostId).HasName("PK_Post");

                entity.Property(e => e.PostId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.GardenerId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.ProductId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Content).IsRequired().HasColumnType("longtext");
                entity.Property(e => e.HarvestDate).HasColumnType("datetime");
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Rating).HasColumnType("decimal(3,2)");
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.PostEndDate).HasColumnType("datetime");
                entity.Property(e => e.Priority).IsRequired();

                entity.HasIndex(e => e.GardenerId).HasDatabaseName("IX_Post_GardenerId");
                entity.HasIndex(e => e.ProductId).HasDatabaseName("IX_Post_CategoryId");

                entity.HasOne(e => e.Account)
                      .WithMany(e => e.Posts)
                      .HasForeignKey(e => e.GardenerId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Post_Account");

                entity.HasOne(e => e.Product)
                      .WithMany(e => e.Posts)
                      .HasForeignKey(e => e.ProductId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Post_Product");
            });

            modelBuilder.Entity<PostMedia>(entity =>
            {
                entity.ToTable("PostMedia");
                entity.HasKey(e => e.PostMediaId).HasName("PK_PostMedia");

                entity.Property(e => e.PostMediaId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.MediumUrl).IsRequired();
                entity.Property(e => e.MediumType).IsRequired().HasMaxLength(20);
                entity.Property(e => e.UploadedAt).HasColumnType("datetime");

                entity.HasOne(e => e.Post)
                      .WithMany(e => e.PostMedias)
                      .HasForeignKey("PostId")
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_PostMedia_Post");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("Report");
                entity.HasKey(e => e.ReportId).HasName("PK_Report");

                entity.Property(e => e.ReportId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.AccountId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.TargetId).HasColumnType("char(26)")
                   .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                   .IsFixedLength();
                entity.Property(e => e.ReportType).IsRequired().HasMaxLength(30);
                entity.Property(e => e.TargetType).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Subject).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Description).HasColumnType("text");
                entity.Property(e => e.Severity).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasIndex(e => e.AccountId).HasDatabaseName("IX_Report_AccountId");

                entity.HasOne(e => e.Account)
                      .WithMany(e => e.Reports)
                      .HasForeignKey(e => e.AccountId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_Report_Account");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Review");
                entity.HasKey(e => e.ReviewId).HasName("PK_Review");

                entity.Property(e => e.ReviewId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.RetailerId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.OrderItemId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.Rating).IsRequired();
                entity.Property(e => e.Comment).HasMaxLength(1000);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.HasIndex(e => e.RetailerId).HasDatabaseName("IX_Review_RetailerId");
                entity.HasIndex(e => e.OrderItemId).HasDatabaseName("IX_Review_OrderItemId");

                entity.HasOne(e => e.Account)
                      .WithMany(e => e.Reviews)
                      .HasForeignKey(e => e.RetailerId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Review_Account");

                entity.HasOne(e => e.OrderItem)
                      .WithMany(e => e.Reviews)
                      .HasForeignKey(e => e.OrderItemId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_Review_OrderItem");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");
                entity.HasKey(e => e.RoleId).HasName("PK_Role");

                entity.Property(e => e.RoleId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(255);
            });

            modelBuilder.Entity<ServiceFeature>(entity =>
            {
                entity.ToTable("ServiceFeature");
                entity.HasKey(e => e.ServiceFeatureId).HasName("PK_ServiceFeature");

                entity.Property(e => e.ServiceFeatureId)
                      .HasColumnType("char(26)")
                      .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                      .IsFixedLength();
                entity.Property(e => e.ServiceFeatureName)
                      .IsRequired()
                      .HasMaxLength(255);
                entity.Property(e => e.Description)
                      .HasColumnType("text");
                entity.Property(e => e.DefaultValue)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.Status)
                      .IsRequired()
                      .HasMaxLength(20);
            });

            modelBuilder.Entity<ServicePackage>(entity =>
            {
                entity.ToTable("ServicePackage");
                entity.HasKey(e => e.ServicePackageId).HasName("PK_ServicePackage");

                entity.Property(e => e.ServicePackageId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.PackageName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Description).HasColumnType("text");
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Duration).IsRequired();
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<SubscriptionContract>(entity =>
            {
                entity.ToTable("SubscriptionContract");
                entity.HasKey(e => e.SubscriptionId).HasName("PK_SubscriptionContract");

                entity.Property(e => e.SubscriptionId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.GardenerId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.ServicePackageId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.StartDate).HasColumnType("datetime");
                entity.Property(e => e.EndDate).HasColumnType("datetime");
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.SubscriptionType).IsRequired().HasMaxLength(20);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.HasIndex(e => e.GardenerId).HasDatabaseName("IX_SubscriptionContract_GardenerId");
                entity.HasIndex(e => e.ServicePackageId).HasDatabaseName("IX_SubscriptionContract_ServicePackageId");

                entity.HasOne(e => e.Account)
                      .WithMany(e => e.ServicePackageContracts)
                      .HasForeignKey(e => e.GardenerId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_SubscriptionContract_Account");

                entity.HasOne(e => e.ServicePackage)
                      .WithMany(e => e.SubscriptionContracts)
                      .HasForeignKey(e => e.ServicePackageId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_SubscriptionContract_ServicePackage");
            });

            modelBuilder.Entity<PackageServiceFeature>(entity =>
            {
                entity.ToTable("PackageServiceFeature");
                entity.HasKey(e => e.PackageServiceFeatureId).HasName("PK_PackageServiceFeature");

                entity.Property(e => e.PackageServiceFeatureId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.ServicePackageId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.ServiceFeatureId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();

                entity.HasIndex(e => e.ServicePackageId).HasDatabaseName("IX_PackageServiceFeature_ServicePackageId");
                entity.HasIndex(e => e.ServiceFeatureId).HasDatabaseName("IX_PackageServiceFeature_ServiceFeatureId");

                entity.HasOne(e => e.ServiceFeature)
                      .WithMany(e => e.ServicePackageFeatures)
                      .HasForeignKey(e => e.ServiceFeatureId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_PackageServiceFeature_ServiceFeature");

                entity.HasOne(e => e.ServicePackage)
                      .WithMany(e => e.ServicePackageFeatures)
                      .HasForeignKey(e => e.ServicePackageId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_PackageServiceFeature_ServicePackage");
            });

            modelBuilder.Entity<ServicePackageOrder>(entity =>
            {
                entity.ToTable("ServicePackageOrder");
                entity.HasKey(e => e.ServicePackageOrderId).HasName("PK_ServicePackageOrder");

                entity.Property(e => e.ServicePackageOrderId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.GardenerId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.ServicePackageId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.HasIndex(e => e.GardenerId).HasDatabaseName("IX_ServicePackageOrder_GardenerId");
                entity.HasIndex(e => e.ServicePackageId).HasDatabaseName("IX_ServicePackageOrder_ServicePackageId");

                entity.HasOne(e => e.ServicePackage)
                      .WithMany(e => e.ServicePackageOrders)
                      .HasForeignKey(e => e.ServicePackageId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_ServicePackageOrder_ServicePackage");

                entity.HasOne(e => e.Gardener)
                      .WithMany(e => e.ServicePackageOrders)
                      .HasForeignKey(e => e.GardenerId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_ServicePackageOrder_Account");
            });

            modelBuilder.Entity<ServicePackageOrderPayment>(entity =>
            {
                entity.ToTable("ServicePackageOrderPayment");
                entity.HasKey(e => e.ServicePackageOrderPaymentId).HasName("PK_ServicePackageOrderPayment");

                entity.Property(e => e.ServicePackageOrderPaymentId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.ServicePackageOrderId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.PaymentAmount).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Currency).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.PaymentDate).HasColumnType("datetime");
                entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(20);

                entity.HasIndex(e => e.ServicePackageOrderId).HasDatabaseName("IX_ServicePackageOrderPayment_ServicePackageOrderId");

                entity.HasOne(e => e.ServicePackageOrder)
                      .WithMany(e => e.ServicePackageOrderPayments)
                      .HasForeignKey(e => e.ServicePackageOrderId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_ServicePackageOrderPayment_ServicePackageOrder");
            });

            modelBuilder.Entity<GardenerIncome>(entity =>
            {
                entity.ToTable("GardenerIncome");
                entity.HasKey(e => e.GardenerIncomeId).HasName("PK_GardenerIncome");

                entity.Property(e => e.GardenerIncomeId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(14,2)");
                entity.Property(e => e.Currency).IsRequired().HasMaxLength(10);
                entity.Property(e => e.TransactionDate).HasColumnType("datetime");
                entity.Property(e => e.GardenerId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();

                entity.HasIndex(e => e.GardenerId).HasDatabaseName("IX_GardenerIncome_GardenerId");

                entity.HasOne(e => e.Account)
                      .WithMany(e => e.GardenerIncomes)
                      .HasForeignKey(e => e.GardenerId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_GardenerIncome_Account");
            });

            modelBuilder.Entity<ProductPrice>(entity =>
            {
                entity.ToTable("ProductPrice");
                entity.HasKey(e => e.ProductPriceId).HasName("PK_ProductPrice");

                entity.Property(e => e.ProductPriceId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Currency).IsRequired().HasMaxLength(10);
                entity.Property(e => e.AvailabledDate).HasColumnType("datetime");
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
                entity.Property(e => e.Productd).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();

                entity.HasIndex(e => e.Productd).HasDatabaseName("IX_ProductPrice_ProductId");

                entity.HasOne(e => e.Product)
                      .WithMany(e => e.ProductPrices)
                      .HasForeignKey(e => e.Productd)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_ProductPrice_Product");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");
                entity.HasKey(e => e.ProductId).HasName("PK_Product");

                entity.Property(e => e.ProductId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.ProductName).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
                entity.Property(e => e.ProductCategoryId).HasColumnType("char(26)")
                    .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                    .IsFixedLength();
                entity.Property(e => e.GardenerId).HasColumnType("char(26)")
                   .HasConversion(ulid => ulid.ToString(), str => Ulid.Parse(str))
                   .IsFixedLength();

                entity.HasIndex(e => e.ProductName).HasDatabaseName("IX_Product_ProductName");

                entity.HasOne(e => e.ProductCategory)
                     .WithMany(e => e.Products)
                     .HasForeignKey(e => e.ProductCategoryId)
                     .OnDelete(DeleteBehavior.Restrict)
                     .HasConstraintName("FK_Product_ProductCategory");

                entity.HasOne(e => e.Gardener)
                     .WithMany(e => e.Products)
                     .HasForeignKey(e => e.GardenerId)
                     .OnDelete(DeleteBehavior.Restrict)
                     .HasConstraintName("FK_Product_GardenerAccount");
            });
        }
    }
}
