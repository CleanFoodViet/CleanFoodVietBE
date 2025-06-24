using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanFoodVietAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateSubscriptionContractBenefit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionContractBenefit_SubscriptionContractId",
                table: "SubscriptionContractBenefit",
                column: "SubscriptionContractId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubscriptionContractBenefit");
        }
    }
}
