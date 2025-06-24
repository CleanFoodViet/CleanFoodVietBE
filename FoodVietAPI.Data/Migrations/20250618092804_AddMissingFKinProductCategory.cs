using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanFoodVietAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingFKinProductCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GardenerId",
                table: "ProductCategory",
                type: "char(26)",
                fixedLength: true,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_GardenderId",
                table: "ProductCategory",
                column: "GardenerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_Gardener",
                table: "ProductCategory",
                column: "GardenerId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_Gardener",
                table: "ProductCategory");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategory_GardenderId",
                table: "ProductCategory");

            migrationBuilder.DropColumn(
                name: "GardenerId",
                table: "ProductCategory");
        }
    }
}
