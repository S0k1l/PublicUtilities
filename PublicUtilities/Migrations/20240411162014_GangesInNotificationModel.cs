using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicUtilities.Migrations
{
    /// <inheritdoc />
    public partial class GangesInNotificationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_PlacesOfResidence_PlacesOfResidenceId",
                table: "Notifications");

            migrationBuilder.AlterColumn<int>(
                name: "PlacesOfResidenceId",
                table: "Notifications",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_PlacesOfResidence_PlacesOfResidenceId",
                table: "Notifications",
                column: "PlacesOfResidenceId",
                principalTable: "PlacesOfResidence",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_PlacesOfResidence_PlacesOfResidenceId",
                table: "Notifications");

            migrationBuilder.AlterColumn<int>(
                name: "PlacesOfResidenceId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_PlacesOfResidence_PlacesOfResidenceId",
                table: "Notifications",
                column: "PlacesOfResidenceId",
                principalTable: "PlacesOfResidence",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
