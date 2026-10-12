using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workspace_Management_System.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dssdasdds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Employees",
                newName: "LastNameEn");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Employees",
                newName: "LastNameAr");

            migrationBuilder.AddColumn<string>(
                name: "FirstNameAr",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstNameEn",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstNameAr",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "FirstNameEn",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "LastNameEn",
                table: "Employees",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "LastNameAr",
                table: "Employees",
                newName: "FirstName");
        }
    }
}
