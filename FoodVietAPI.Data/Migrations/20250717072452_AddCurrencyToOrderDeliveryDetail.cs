using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanFoodVietAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyToOrderDeliveryDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GardenerIncome");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "OrderDeliveryDetail",
                type: "varchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "OrderDeliveryDetail");

            migrationBuilder.CreateTable(
                name: "GardenerIncome",
                columns: table => new
                {
                    GardenerIncomeId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    GardenerId = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
                    Currency = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(14,2)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GardenerIncome", x => x.GardenerIncomeId);
                    table.ForeignKey(
                        name: "FK_GardenerIncome_Account",
                        column: x => x.GardenerId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "IX_GardenerIncome_GardenerId",
                table: "GardenerIncome",
                column: "GardenerId");
        }
    }
}
