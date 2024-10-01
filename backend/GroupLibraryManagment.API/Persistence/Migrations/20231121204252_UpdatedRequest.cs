using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupLibraryManagment.API.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PhysicalBookId",
                table: "Requests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Requests",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "ISBN",
                table: "Requests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ISBN",
                table: "Requests",
                column: "ISBN");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_GenericBooks_ISBN",
                table: "Requests",
                column: "ISBN",
                principalTable: "GenericBooks",
                principalColumn: "ISBN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_GenericBooks_ISBN",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ISBN",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ISBN",
                table: "Requests");

            migrationBuilder.AlterColumn<int>(
                name: "PhysicalBookId",
                table: "Requests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Requests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
