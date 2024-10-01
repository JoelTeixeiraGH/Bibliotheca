using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupLibraryManagment.API.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PhysicalBookUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Available",
                table: "PhysicalBooks");

            migrationBuilder.AddColumn<int>(
                name: "PhysicalBookStatus",
                table: "PhysicalBooks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhysicalBookStatus",
                table: "PhysicalBooks");

            migrationBuilder.AddColumn<bool>(
                name: "Available",
                table: "PhysicalBooks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
