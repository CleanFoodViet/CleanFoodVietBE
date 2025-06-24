using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanFoodVietAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAddressLine2inAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "Address");

            migrationBuilder.Sql("ALTER TABLE `Address` CHANGE `AddressLine1` `AddressLine` NVARCHAR(255);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AddressLine",
                table: "Address",
                newName: "AddressLine1");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "Address",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
