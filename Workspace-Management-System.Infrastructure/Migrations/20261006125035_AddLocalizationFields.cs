using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workspace_Management_System.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLocalizationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Services",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Services",
                newName: "DescriptionEn");

            migrationBuilder.RenameColumn(
                name: "EnglishName",
                table: "Products",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Products",
                newName: "DescriptionEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ProductCategories",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ProductCategories",
                newName: "DescriptionEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "PricingPlans",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "PricingPlans",
                newName: "DescriptionEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Packages",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Packages",
                newName: "DescriptionEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Discounts",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Discounts",
                newName: "DescriptionEn");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Customers",
                newName: "NotesEn");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Customers",
                newName: "FullNameEn");

            migrationBuilder.RenameColumn(
                name: "TaxInformation",
                table: "Companies",
                newName: "TaxInformationEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Companies",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "ContractDetails",
                table: "Companies",
                newName: "ContractDetailsEn");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "ProductCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "ProductCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "PricingPlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "PricingPlans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Packages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Packages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Discounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Discounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullNameAr",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NotesAr",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxInformationAr",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContractDetailsAr",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "PricingPlans");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "PricingPlans");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "FullNameAr",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "NotesAr",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "TaxInformationAr",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ContractDetailsAr",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Services",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "Services",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Products",
                newName: "EnglishName");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "Products",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "ProductCategories",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "ProductCategories",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "PricingPlans",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "PricingPlans",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Packages",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "Packages",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Discounts",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "Discounts",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "NotesEn",
                table: "Customers",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "FullNameEn",
                table: "Customers",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "TaxInformationEn",
                table: "Companies",
                newName: "TaxInformation");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Companies",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ContractDetailsEn",
                table: "Companies",
                newName: "ContractDetails");
        }
    }
}