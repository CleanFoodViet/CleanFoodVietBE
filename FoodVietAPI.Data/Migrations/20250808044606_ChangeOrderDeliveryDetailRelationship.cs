using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanFoodVietAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOrderDeliveryDetailRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDeliveryDetail_Product",
                table: "OrderDeliveryDetail");

            //migrationBuilder.RenameColumn(
            //    name: "ProductId",
            //    table: "OrderDeliveryDetail",
            //    newName: "OrderDetailId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_OrderDeliveryDetail_ProductId",
            //    table: "OrderDeliveryDetail",
            //    newName: "IX_OrderDeliveryDetail_OrderDetailId");

            migrationBuilder.Sql(@"
                ALTER TABLE `OrderDeliveryDetail` 
                CHANGE `ProductId` `OrderDetailId` char(26);
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE `OrderDeliveryDetail` 
                RENAME INDEX `IX_OrderDeliveryDetail_ProductId` TO `IX_OrderDeliveryDetail_OrderDetailId`;
            ");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDeliveryDetail_OrderDetail",
                table: "OrderDeliveryDetail",
                column: "OrderDetailId",
                principalTable: "OrderDetail",
                principalColumn: "OrderDetailId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDeliveryDetail_OrderDetail",
                table: "OrderDeliveryDetail");

            migrationBuilder.RenameColumn(
                name: "OrderDetailId",
                table: "OrderDeliveryDetail",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDeliveryDetail_OrderDetailId",
                table: "OrderDeliveryDetail",
                newName: "IX_OrderDeliveryDetail_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDeliveryDetail_Product",
                table: "OrderDeliveryDetail",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
