using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanFoodVietAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToServiceFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ServiceFeature",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ServiceFeature");
        }
    }
}
