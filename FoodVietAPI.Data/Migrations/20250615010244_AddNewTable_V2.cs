using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanFoodVietAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTable_V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemLog");

            migrationBuilder.Sql("ALTER TABLE `OrderItem` CHANGE `ShippingStatus` `DeliveryStatus` VARCHAR(25) NOT NULL;");

            migrationBuilder.CreateTable(
                name: "OrderDelivery",
                columns: table => new
                {
                    OrderDeliveryId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    OrderId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DeliveryStatus = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
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
                name: "OrderDeliveryItem",
                columns: table => new
                {
                    OrderDeliveryItemId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    OrderDeliveryId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    OrderItemId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DeliveredAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDeliveryItem", x => x.OrderDeliveryItemId);
                    table.ForeignKey(
                        name: "FK_OrderDeliveryItem_OrderDelivery",
                        column: x => x.OrderDeliveryId,
                        principalTable: "OrderDelivery",
                        principalColumn: "OrderDeliveryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderDeliveryItem_OrderItem",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItem",
                        principalColumn: "OrderItemId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDelivery_OrderId",
                table: "OrderDelivery",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDeliveryItem_OrderDeliveryId",
                table: "OrderDeliveryItem",
                column: "OrderDeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDeliveryItem_OrderItemId",
                table: "OrderDeliveryItem",
                column: "OrderItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDeliveryItem");

            migrationBuilder.DropTable(
                name: "OrderDelivery");

            migrationBuilder.RenameColumn(
                name: "DeliveryStatus",
                table: "OrderItem",
                newName: "ShippingStatus");

            migrationBuilder.CreateTable(
                name: "SystemLog",
                columns: table => new
                {
                    LogId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    AdminId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    Action = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    Desctiption = table.Column<string>(type: "text", nullable: true),
                    TargetId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    TargetType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemLog", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_SystemLog_Account",
                        column: x => x.AdminId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "IX_SystemLog_AdminId",
                table: "SystemLog",
                column: "AdminId");
        }
    }
}
