using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workspace_Management_System.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class lklkdhdhdhd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Memberships",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Memberships",
                newName: "DescriptionEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "MembershipBenefits",
                newName: "DescriptionEn");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Memberships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Memberships",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "MembershipBenefits",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "MembershipBenefits");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Memberships",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "Memberships",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "MembershipBenefits",
                newName: "Description");
        }
    }
}
