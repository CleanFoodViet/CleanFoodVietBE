using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanFoodVietAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRejectOrderFieldName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE `Order` CHANGE `RejectReason` `CancelReason` varchar(256);");

            //migrationBuilder.RenameColumn(
            //    name: "RejectReason",
            //    table: "Order",
            //    newName: "CancelReason");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CancelReason",
                table: "Order",
                newName: "RejectReason");
        }
    }
}
