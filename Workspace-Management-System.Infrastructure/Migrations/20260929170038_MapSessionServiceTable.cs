using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workspace_Management_System.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MapSessionServiceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionService_Services_ServiceId",
                table: "SessionService");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionService_Sessions_SessionId",
                table: "SessionService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionService",
                table: "SessionService");

            migrationBuilder.RenameTable(
                name: "SessionService",
                newName: "SessionServices");

            migrationBuilder.RenameIndex(
                name: "IX_SessionService_SessionId_ServiceId",
                table: "SessionServices",
                newName: "IX_SessionServices_SessionId_ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_SessionService_ServiceId",
                table: "SessionServices",
                newName: "IX_SessionServices_ServiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionServices",
                table: "SessionServices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionServices_Services_ServiceId",
                table: "SessionServices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionServices_Sessions_SessionId",
                table: "SessionServices",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionServices_Services_ServiceId",
                table: "SessionServices");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionServices_Sessions_SessionId",
                table: "SessionServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionServices",
                table: "SessionServices");

            migrationBuilder.RenameTable(
                name: "SessionServices",
                newName: "SessionService");

            migrationBuilder.RenameIndex(
                name: "IX_SessionServices_SessionId_ServiceId",
                table: "SessionService",
                newName: "IX_SessionService_SessionId_ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_SessionServices_ServiceId",
                table: "SessionService",
                newName: "IX_SessionService_ServiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionService",
                table: "SessionService",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionService_Services_ServiceId",
                table: "SessionService",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionService_Sessions_SessionId",
                table: "SessionService",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
