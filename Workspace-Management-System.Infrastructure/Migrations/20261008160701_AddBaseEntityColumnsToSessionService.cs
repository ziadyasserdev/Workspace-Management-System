using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workspace_Management_System.Infrastructure.Migrations
{
    public partial class AddBaseEntityColumnsToSessionService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SessionService",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SessionService",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SessionService",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "IsDeletedBy",
                table: "SessionService",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "SessionService",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "SessionService",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SessionService");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SessionService");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SessionService");

            migrationBuilder.DropColumn(
                name: "IsDeletedBy",
                table: "SessionService");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "SessionService");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "SessionService");
        }
    }
}