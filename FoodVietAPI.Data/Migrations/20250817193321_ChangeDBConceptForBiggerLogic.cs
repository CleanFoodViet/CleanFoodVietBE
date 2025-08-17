using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanFoodVietAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDBConceptForBiggerLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    ProductCategoryId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => x.ProductCategoryId);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "ServiceFeature",
                columns: table => new
                {
                    ServiceFeatureId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    ServiceFeatureName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DefaultValue = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "longtext", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceFeature", x => x.ServiceFeatureId);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "ServicePackage",
                columns: table => new
                {
                    ServicePackageId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    PackageName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePackage", x => x.ServicePackageId);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Bio = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false),
                    Password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Gender = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    Avatar = table.Column<string>(type: "longtext", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    IsVerified = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    RoleId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Account_Role",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "PackageServiceFeature",
                columns: table => new
                {
                    PackageServiceFeatureId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    ServicePackageId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    ServiceFeatureId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageServiceFeature", x => x.PackageServiceFeatureId);
                    table.ForeignKey(
                        name: "FK_PackageServiceFeature_ServiceFeature",
                        column: x => x.ServiceFeatureId,
                        principalTable: "ServiceFeature",
                        principalColumn: "ServiceFeatureId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PackageServiceFeature_ServicePackage",
                        column: x => x.ServicePackageId,
                        principalTable: "ServicePackage",
                        principalColumn: "ServicePackageId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    AddressId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    AddressLine = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    City = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Province = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    PostalCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Country = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Longitude = table.Column<double>(type: "double", nullable: false),
                    Latitude = table.Column<double>(type: "double", nullable: false),
                    AccountId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_Address_Account",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    AppointmentId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    GardenerId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    RetailerId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Location = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    AppointmentType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    ActionedBy = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    ActionReason = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_Appointment_Gardeners",
                        column: x => x.GardenerId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointment_Retailers",
                        column: x => x.RetailerId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    CartId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    RetailerId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    GardenerId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_Cart_Gardeners",
                        column: x => x.GardenerId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cart_Retailers",
                        column: x => x.RetailerId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Certificate",
                columns: table => new
                {
                    CertificateId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: false),
                    IssuingAuthority = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    GardenerId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificate", x => x.CertificateId);
                    table.ForeignKey(
                        name: "FK_Certificate_Account",
                        column: x => x.GardenerId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "ChatMessage",
                columns: table => new
                {
                    ChatMessageId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    SenderId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    ReceiverId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    MessageText = table.Column<string>(type: "text", nullable: true),
                    SentAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    MessageStatus = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessage", x => x.ChatMessageId);
                    table.ForeignKey(
                        name: "FK_ChatMessage_Receiver",
                        column: x => x.ReceiverId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatMessage_Sender",
                        column: x => x.SenderId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    AccountId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    Message = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    Link = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Sender = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "System"),
                    IsRead = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notification_Account",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    RetailerId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    GardenerId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(14,2)", nullable: false),
                    ShippingCost = table.Column<decimal>(type: "decimal(14,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    CancelReason = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    ShippingAddress = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    TotalDepositAmount = table.Column<decimal>(type: "decimal(14,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Order_Gardeners",
                        column: x => x.GardenerId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Retailers",
                        column: x => x.RetailerId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    ProductName = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    ProductCategoryId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    GardenerId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    HarvestStatus = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Product_GardenerAccount",
                        column: x => x.GardenerId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Product_ProductCategory",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategory",
                        principalColumn: "ProductCategoryId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    ReportId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    ReportType = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    TargetId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    TargetType = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Subject = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Severity = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    AccountId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Report_Account",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "ServicePackageOrder",
                columns: table => new
                {
                    ServicePackageOrderId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    GardenerId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    ServicePackageId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePackageOrder", x => x.ServicePackageOrderId);
                    table.ForeignKey(
                        name: "FK_ServicePackageOrder_Account",
                        column: x => x.GardenerId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServicePackageOrder_ServicePackage",
                        column: x => x.ServicePackageId,
                        principalTable: "ServicePackage",
                        principalColumn: "ServicePackageId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "SubscriptionContract",
                columns: table => new
                {
                    SubscriptionId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    GardenerId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    ServicePackageId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DurationInDays = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    SubscriptionType = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionContract", x => x.SubscriptionId);
                    table.ForeignKey(
                        name: "FK_SubscriptionContract_Account",
                        column: x => x.GardenerId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubscriptionContract_ServicePackage",
                        column: x => x.ServicePackageId,
                        principalTable: "ServicePackage",
                        principalColumn: "ServicePackageId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "OrderDelivery",
                columns: table => new
                {
                    OrderDeliveryId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    OrderId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DeliveryStatus = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    Note = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(14,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDelivery", x => x.OrderDeliveryId);
                    table.ForeignKey(
                        name: "FK_OrderDelivery_Order",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "CartItem",
                columns: table => new
                {
                    CartItemId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    CartId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    ProductId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(14,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ProductUnit = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    DepositAmount = table.Column<decimal>(type: "decimal(14,2)", nullable: false),
                    DepositPercentage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItem", x => x.CartItemId);
                    table.ForeignKey(
                        name: "FK_CartItem_Cart",
                        column: x => x.CartId,
                        principalTable: "Cart",
                        principalColumn: "CartId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItem_Product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    OrderDetailId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    OrderId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(14,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ProductUnit = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    DeliveryStatus = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    ProductId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    DepositAmount = table.Column<decimal>(type: "decimal(14,2)", nullable: false),
                    DepositPercentage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.OrderDetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Order",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    PostId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    GardenerId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Content = table.Column<string>(type: "longtext", nullable: false),
                    HarvestDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ProductId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    PostEndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    DepositAmount = table.Column<decimal>(type: "decimal(14,2)", nullable: false),
                    DepositPercentage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Post_Account",
                        column: x => x.GardenerId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Post_Product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "ProductCertificate",
                columns: table => new
                {
                    ProductCertificateId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    ProductId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    CertificateName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    IssuingOrganization = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    CertificateNumber = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCertificate", x => x.ProductCertificateId);
                    table.ForeignKey(
                        name: "FK_ProductCertificate_Product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "ProductPrice",
                columns: table => new
                {
                    ProductPriceId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Currency = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    WeightUnit = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    AvailabledDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsCurrent = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Productd = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPrice", x => x.ProductPriceId);
                    table.ForeignKey(
                        name: "FK_ProductPrice_Product",
                        column: x => x.Productd,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "ProductTag",
                columns: table => new
                {
                    ProductTagId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    ProductId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    GardenerId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    TagName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTag", x => x.ProductTagId);
                    table.ForeignKey(
                        name: "FK_ProductTag_Gardener",
                        column: x => x.GardenerId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductTag_Product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "ServicePackageOrderPayment",
                columns: table => new
                {
                    ServicePackageOrderPaymentId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    ServicePackageOrderId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Currency = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    PaymentMethod = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePackageOrderPayment", x => x.ServicePackageOrderPaymentId);
                    table.ForeignKey(
                        name: "FK_ServicePackageOrderPayment_ServicePackageOrder",
                        column: x => x.ServicePackageOrderId,
                        principalTable: "ServicePackageOrder",
                        principalColumn: "ServicePackageOrderId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "SubscriptionContractBenefit",
                columns: table => new
                {
                    SubscriptionContractBenefitId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    SubscriptionContractId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    BenefitType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    DefaultValue = table.Column<int>(type: "int", nullable: false),
                    RemainingValue = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionContractBenefit", x => x.SubscriptionContractBenefitId);
                    table.ForeignKey(
                        name: "FK_SubscriptionContractBenefit_SubscriptionContract",
                        column: x => x.SubscriptionContractId,
                        principalTable: "SubscriptionContract",
                        principalColumn: "SubscriptionId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "OrderDeliveryDetail",
                columns: table => new
                {
                    OrderDeliveryDetailId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    OrderDeliveryId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    OrderDetailId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    DeliveredAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(14,2)", nullable: false),
                    ProductUnit = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    Currency = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDeliveryDetail", x => x.OrderDeliveryDetailId);
                    table.ForeignKey(
                        name: "FK_OrderDeliveryDetail_OrderDetail",
                        column: x => x.OrderDetailId,
                        principalTable: "OrderDetail",
                        principalColumn: "OrderDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderDeliveryItem_OrderDelivery",
                        column: x => x.OrderDeliveryId,
                        principalTable: "OrderDelivery",
                        principalColumn: "OrderDeliveryId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    ReviewId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    RetailerId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    OrderDetailId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Review_Account",
                        column: x => x.RetailerId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Review_OrderDetail",
                        column: x => x.OrderDetailId,
                        principalTable: "OrderDetail",
                        principalColumn: "OrderDetailId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Favorite",
                columns: table => new
                {
                    FavoriteId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    RetailerId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    PostId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorite", x => x.FavoriteId);
                    table.ForeignKey(
                        name: "FK_Favorite_Account",
                        column: x => x.RetailerId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorite_Post",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "PostMedia",
                columns: table => new
                {
                    PostMediaId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    MediumUrl = table.Column<string>(type: "longtext", nullable: false),
                    MediumType = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    PostId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostMedia", x => x.PostMediaId);
                    table.ForeignKey(
                        name: "FK_PostMedia_Post",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Account_Email",
                table: "Account",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_PhoneNumber",
                table: "Account",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_RoleId",
                table: "Account",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_UserId",
                table: "Address",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_GardenerId",
                table: "Appointment",
                column: "GardenerId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_RetailerId",
                table: "Appointment",
                column: "RetailerId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_GardenerId",
                table: "Cart",
                column: "GardenerId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_RetailerId",
                table: "Cart",
                column: "RetailerId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CartId",
                table: "CartItem",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_PostId",
                table: "CartItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_GardenerId",
                table: "Certificate",
                column: "GardenerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_ReceiverId",
                table: "ChatMessage",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_SenderId",
                table: "ChatMessage",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorite_PostId",
                table: "Favorite",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorite_RetailerId",
                table: "Favorite",
                column: "RetailerId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_AccountId",
                table: "Notification",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_GardenerId",
                table: "Order",
                column: "GardenerId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_RetailerId",
                table: "Order",
                column: "RetailerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDelivery_OrderId",
                table: "OrderDelivery",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDeliveryDetail_OrderDeliveryId",
                table: "OrderDeliveryDetail",
                column: "OrderDeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDeliveryDetail_OrderDetailId",
                table: "OrderDeliveryDetail",
                column: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderId",
                table: "OrderDetail",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ProductId",
                table: "OrderDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageServiceFeature_ServiceFeatureId",
                table: "PackageServiceFeature",
                column: "ServiceFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageServiceFeature_ServicePackageId",
                table: "PackageServiceFeature",
                column: "ServicePackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_CategoryId",
                table: "Post",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_GardenerId",
                table: "Post",
                column: "GardenerId");

            migrationBuilder.CreateIndex(
                name: "IX_PostMedia_PostId",
                table: "PostMedia",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_GardenerId",
                table: "Product",
                column: "GardenerId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductCategoryId",
                table: "Product",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductName",
                table: "Product",
                column: "ProductName");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionContractBenefit_SubscriptionContractId",
                table: "ProductCertificate",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrice_ProductId",
                table: "ProductPrice",
                column: "Productd");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTag_GardenerId",
                table: "ProductTag",
                column: "GardenerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTag_ProductId",
                table: "ProductTag",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_AccountId",
                table: "Report",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_OrderDetailId",
                table: "Review",
                column: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_RetailerId",
                table: "Review",
                column: "RetailerId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackageOrder_GardenerId",
                table: "ServicePackageOrder",
                column: "GardenerId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackageOrder_ServicePackageId",
                table: "ServicePackageOrder",
                column: "ServicePackageId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackageOrderPayment_ServicePackageOrderId",
                table: "ServicePackageOrderPayment",
                column: "ServicePackageOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionContract_GardenerId",
                table: "SubscriptionContract",
                column: "GardenerId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionContract_ServicePackageId",
                table: "SubscriptionContract",
                column: "ServicePackageId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionContractBenefit_SubscriptionContractId",
                table: "SubscriptionContractBenefit",
                column: "SubscriptionContractId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "CartItem");

            migrationBuilder.DropTable(
                name: "Certificate");

            migrationBuilder.DropTable(
                name: "ChatMessage");

            migrationBuilder.DropTable(
                name: "Favorite");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "OrderDeliveryDetail");

            migrationBuilder.DropTable(
                name: "PackageServiceFeature");

            migrationBuilder.DropTable(
                name: "PostMedia");

            migrationBuilder.DropTable(
                name: "ProductCertificate");

            migrationBuilder.DropTable(
                name: "ProductPrice");

            migrationBuilder.DropTable(
                name: "ProductTag");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "ServicePackageOrderPayment");

            migrationBuilder.DropTable(
                name: "SubscriptionContractBenefit");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "OrderDelivery");

            migrationBuilder.DropTable(
                name: "ServiceFeature");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "ServicePackageOrder");

            migrationBuilder.DropTable(
                name: "SubscriptionContract");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "ServicePackage");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "ProductCategory");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
