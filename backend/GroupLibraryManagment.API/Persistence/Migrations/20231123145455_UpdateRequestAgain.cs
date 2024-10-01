using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupLibraryManagment.API.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRequestAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LibraryId",
                table: "Requests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_LibraryId",
                table: "Requests",
                column: "LibraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Libraries_LibraryId",
                table: "Requests",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "LibraryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Libraries_LibraryId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_LibraryId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "LibraryId",
                table: "Requests");
        }
    }
}
