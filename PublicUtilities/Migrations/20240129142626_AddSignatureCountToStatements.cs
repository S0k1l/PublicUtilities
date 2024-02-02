using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicUtilities.Migrations
{
    /// <inheritdoc />
    public partial class AddSignatureCountToStatements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SignarureCount",
                table: "Statements",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignarureCount",
                table: "Statements");
        }
    }
}
