using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanFoodVietAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRejectReasonToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RejectReason",
                table: "Order",
                type: "varchar(256)",
                maxLength: 256,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectReason",
                table: "Order");
        }
    }
}
