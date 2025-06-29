using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanFoodVietAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class DurationInContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationInDays",
                table: "SubscriptionContract",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationInDays",
                table: "SubscriptionContract");
        }
    }
}
