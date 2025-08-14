using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanFoodVietAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAppointmentActionField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
            //    name: "CancelledBy",
            //    table: "Appointment",
            //    newName: "ActionedBy");

            //migrationBuilder.RenameColumn(
            //    name: "CancellationReason",
            //    table: "Appointment",
            //    newName: "ActionReason");

            migrationBuilder.Sql(
                "ALTER TABLE `Appointment` CHANGE `CancelledBy` `ActionedBy` VARCHAR(150);"
            );

            migrationBuilder.Sql(
                "ALTER TABLE `Appointment` CHANGE `CancellationReason` `ActionReason` VARCHAR(255);"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActionedBy",
                table: "Appointment",
                newName: "CancelledBy");

            migrationBuilder.RenameColumn(
                name: "ActionReason",
                table: "Appointment",
                newName: "CancellationReason");
        }
    }
}
