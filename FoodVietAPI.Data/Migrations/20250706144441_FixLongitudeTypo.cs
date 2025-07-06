using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanFoodVietAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixLongitudeTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
            //    name: "Longtitude",
            //    table: "Address",
            //    newName: "Longitude");

            migrationBuilder.Sql("ALTER TABLE `Address` CHANGE `Longtitude` `Longitude` double;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Address",
                newName: "Longtitude");
        }
    }
}
